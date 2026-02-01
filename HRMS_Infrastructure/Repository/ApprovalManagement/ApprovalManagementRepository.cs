using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.ApprovalManagement;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Probations;
using HRMS_Infrastructure.Interface.ApprovalManagement;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.ApprovalManagement
{
    public class ApprovalManagementRepository : IApprovalManagementRepository
    {
        private readonly string _connectionString;

        public ApprovalManagementRepository(HRMSDbContext db)
        {
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }


        public async Task<APIResponse> GetApprovalDropdownDetails()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var multi = await connection.QueryMultipleAsync(
                        "GetApprovalDropdownDetails",
                        commandType: CommandType.StoredProcedure))
                    {
                        var result = new ApprovalDropdownViewModel
                        {
                            Companies = (await multi.ReadAsync<CompanyViewModel>()).AsList(),
                            ApprovalTypes = (await multi.ReadAsync<ApprovalTypeViewModel>()).AsList(),
                            Designations = (await multi.ReadAsync<DesignationViewModel>()).AsList(),
                            Departments = (await multi.ReadAsync<DepartmentViewModel>()).AsList()
                        };

                        return new APIResponse { Data = result, ResponseMessage = "Fetched successfully!", isSuccess = true };
                    }
                }
            }
            catch (Exception)
            {
                return new APIResponse { ResponseMessage = "Something went wrong!", isSuccess = false };
            }
        }


        public async Task<APIResponse> GetAllEmployByDepartmentId(int? companyId, int? departmentId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var employees = await connection.QueryAsync<dynamic>(
                        "GetAllEmployByDepartmentId",
                        new { CompanyId = companyId, DepartmentId = departmentId },
                        commandType: CommandType.StoredProcedure);

                    return new APIResponse
                    {
                        Data = employees.AsList(),
                        ResponseMessage = "Fetched successfully!",
                        isSuccess = true
                    };
                }
            }
            catch (Exception)
            {
                return new APIResponse
                {
                    ResponseMessage = "Something went wrong!",
                    isSuccess = false
                };
            }
        }

        // -------------------------------------------------------------
        // APPROVAL SCHEME LEVEL
        // -------------------------------------------------------------
        public async Task<APIResponse> ManageApprovalLevel(List<ApprovalLevelVM> models)
        {
            var response = new APIResponse();
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    var transaction = con.BeginTransaction();
                    try
                    {
                        foreach (var model in models)
                        {
                            // Handle ApprovalLevel
                            var p = new DynamicParameters();
                            p.Add("@Action", "Upsert");
                            p.Add("@ApprovalLevelId", model.ApprovalLevelId);
                            p.Add("@ApprovalMasterId", model.ApprovalMasterId);
                            p.Add("@LevelNo", model.LevelNo);
                            p.Add("@ApproverEmployeeId", model.ApproverEmployeeId);
                            p.Add("@IsReportingPerson", model.IsReportingPerson);
                            p.Add("@IsHR", model.IsHR);
                            p.Add("@IsNationalManager", model.IsNationalManager);
                            p.Add("@IsHOD", model.IsHOD);
                            p.Add("@IsDepartmentBased", model.IsDepartmentBased);
                            p.Add("@EscalationDays", model.EscalationDays);
                            p.Add("@CreatedBy", model.CreatedBy);
                            p.Add("@UpdatedBy", model.UpdatedBy);
                            p.Add("@DeletedBy", model.DeletedBy);
                            p.Add("@CompanyId", model.CompanyId);
                            p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                            p.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 300);
                            p.Add("@ReturnId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                            await con.ExecuteAsync(
                                "sp_Manage_ApprovalLevel",
                                p,
                                commandType: CommandType.StoredProcedure,
                                transaction: transaction
                            );

                            var success = p.Get<bool>("@Success");
                            var message = p.Get<string>("@Message");
                            var returnId = p.Get<int>("@ReturnId");

                            if (!success)
                            {
                                response.isSuccess = false;
                                response.ResponseMessage = message;
                                return response;
                            }

                            // Handle ApprovalLevelDepartment if IsDepartmentBased is true
                            if (model.IsDepartmentBased && model.ApprovalLevelDepartments != null && model.ApprovalLevelDepartments.Any())
                            {
                                foreach (var dept in model.ApprovalLevelDepartments)
                                {
                                    var deptParams = new DynamicParameters();
                                    deptParams.Add("@ApprovalLevelId", returnId);
                                    deptParams.Add("@DepartmentId", dept.DepartmentId);
                                    deptParams.Add("@ApproverEmployeeId", dept.ApproverEmployeeId);
                                    deptParams.Add("@IsNotMandatory", dept.IsNotMandatory);
                                    deptParams.Add("@IsActive", true);
                                    deptParams.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
                                    deptParams.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                                    await con.ExecuteAsync(
                                        "usp_UpsertApprovalLevelDepartment",
                                        deptParams,
                                        commandType: CommandType.StoredProcedure,
                                        transaction: transaction
                                    );

                                    var deptSuccess = deptParams.Get<bool>("@Success");
                                    var deptMessage = deptParams.Get<string>("@ResponseMessage");

                                    if (!deptSuccess)
                                    {
                                        response.isSuccess = false;
                                        response.ResponseMessage = deptMessage;
                                        return response;
                                    }
                                }
                            }
                        }
                        transaction.Commit();
                        response.isSuccess = true;
                        response.ResponseMessage = "All records processed successfully.";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        response.isSuccess = false;
                        response.ResponseMessage = ex.Message;
                    }
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        // -------------------------------------------------------------
        // APPROVAL REQUEST
        // -------------------------------------------------------------
        public async Task<APIResponse> ManageApprovalRequest(ApprovalRequestVM model)
        {
            var response = new APIResponse();

            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var p = new DynamicParameters();
                    p.Add("@Action", model.Action);
                    p.Add("@ApprovalRequestId", model.ApprovalRequestId);

                    p.Add("@ApprovalSchemeId", model.ApprovalSchemeId);
                    p.Add("@RequesterEmployeeId", model.RequesterEmployeeId);
                    p.Add("@RequesterDepartmentId", model.RequesterDepartmentId);
                    p.Add("@RequestTitle", model.RequestTitle);
                    p.Add("@RequestData", model.RequestData);
                    p.Add("@CurrentLevelSeq", model.CurrentLevelSeq);
                    p.Add("@Status", model.Status);

                    p.Add("@CreatedBy", model.CreatedBy);
                    p.Add("@UpdatedBy", model.UpdatedBy);
                    p.Add("@DeletedBy", model.DeletedBy);

                    var result = await con.QueryFirstAsync<dynamic>(
                        "sp_Manage_ApprovalRequest",
                        p,
                        commandType: CommandType.StoredProcedure
                    );

                    response.isSuccess = result.Success;
                    response.ResponseMessage = result.ResponseMessage;
                    response.Data = result.ApprovalRequestId;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }

            return response;
        }

        public async Task<APIResponse> DeleteApprovalLevel(ApprovalLevelPara para)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Action", "Delete");
                    parameters.Add("@ApprovalMasterId", para.ApprovalMasterId);
                    parameters.Add("@ApprovalLevelId", para.ApprovalLevelId);
                    parameters.Add("@DeletedBy", para.DeletedBy);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 300);
                    parameters.Add("@ReturnId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync(
                        "sp_Manage_ApprovalLevel",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    var success = parameters.Get<bool>("@Success");
                    var message = parameters.Get<string>("@Message");
                    var returnId = parameters.Get<int>("@ReturnId");

                    return new APIResponse
                    {
                        isSuccess = success,
                        ResponseMessage = message,
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Error: {ex.Message}"
                };
            }
        }

        // -------------------------------------------------------------
        // APPROVAL REQUEST LEVEL
        // -------------------------------------------------------------
        public async Task<APIResponse> ManageApprovalRequestLevel(ApprovalRequestLevelVM model)
        {
            var response = new APIResponse();

            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var p = new DynamicParameters();

                    p.Add("@Action", model.Action);
                    p.Add("@ApprovalRequestLevelId", model.ApprovalRequestLevelId);

                    p.Add("@ApprovalRequestId", model.ApprovalRequestId);
                    p.Add("@ApprovalSchemeLevelId", model.ApprovalSchemeLevelId);
                    p.Add("@SequenceNo", model.SequenceNo);
                    p.Add("@ApproverEmployeeId", model.ApproverEmployeeId);

                    p.Add("@Status", model.Status);
                    p.Add("@ActionRemarks", model.ActionRemarks);
                    p.Add("@ActionBy", model.ActionBy);
                    p.Add("@ActionOn", model.ActionOn);

                    p.Add("@EscalatedOn", model.EscalatedOn);
                    p.Add("@EscalationDueOn", model.EscalationDueOn);

                    p.Add("@CreatedBy", model.CreatedBy);
                    p.Add("@UpdatedBy", model.UpdatedBy);
                    p.Add("@DeletedBy", model.DeletedBy);

                    var result = await con.QueryFirstAsync<dynamic>(
                        "sp_Manage_ApprovalRequestLevel",
                        p,
                        commandType: CommandType.StoredProcedure
                    );

                    response.isSuccess = result.Success;
                    response.ResponseMessage = result.ResponseMessage;
                    response.Data = result.ApprovalRequestLevelId;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }

            return response;
        }

        // -------------------------------------------------------------
        // APPROVAL REQUEST HISTORY
        // -------------------------------------------------------------
        public async Task<APIResponse> InsertApprovalRequestHistory(ApprovalRequestHistoryVM model)
        {
            var response = new APIResponse();

            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var p = new DynamicParameters();

                    p.Add("@ApprovalRequestId", model.ApprovalRequestId);
                    p.Add("@ApprovalRequestLevelId", model.ApprovalRequestLevelId);
                    p.Add("@ActionType", model.ActionType);
                    p.Add("@ActionBy", model.ActionBy);
                    p.Add("@Remarks", model.Remarks);
                    p.Add("@OldStatus", model.OldStatus);
                    p.Add("@NewStatus", model.NewStatus);
                    p.Add("@CreatedBy", model.CreatedBy);

                    p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    p.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

                    await con.ExecuteAsync(
                        "Sp_ApprovalRequestHistory_Insert",
                        p,
                        commandType: CommandType.StoredProcedure
                    );

                    response.isSuccess = p.Get<bool>("@Success");
                    response.ResponseMessage = p.Get<string>("@ResponseMessage");
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }

            return response;
        }

        // -------------------------------------------------------------
        // APPROVE / REJECT REQUEST LEVEL
        // -------------------------------------------------------------
        public async Task<APIResponse> ActionOnApprovalRequestLevel(ApprovalRequestLevelActionVM model)
        {
            var response = new APIResponse();

            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    var p = new DynamicParameters();

                    p.Add("@ApprovalRequestId", model.ApprovalRequestId);
                    p.Add("@ApprovalSchemeLevelId", model.ApprovalSchemeLevelId);
                    p.Add("@Action", model.Action);
                    p.Add("@ActionBy", model.ActionBy);
                    p.Add("@Remarks", model.Remarks);

                    var result = await con.QueryFirstAsync<dynamic>(
                        "sp_ApprovalRequestLevel_Action",
                        p,
                        commandType: CommandType.StoredProcedure
                    );

                    response.isSuccess = true;
                    response.ResponseMessage = "Action executed successfully.";
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }

            return response;
        }


     
        public async Task<APIResponse> GetAllApprovalLevelsByCompanyId(int companyId)
        {
            var response = new APIResponse();
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompanyId", companyId, DbType.Int32);

                    var result = await con.QueryAsync<GetApprovalLevelVM>(
                        "usp_GetAllApprovalLevelsByCompanyId",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    // Deserialize the JSON string for approvalSchemeLevelDepartmentVMs
                    foreach (var level in result)
                    {
                        if (!string.IsNullOrEmpty(level.approvalLevelDepartmentVMsJson))
                        {
                            level.approvalLevelDepartmentVMs = JsonConvert.DeserializeObject<List<GetApprovalLevelDepartmentVM>>(level.approvalLevelDepartmentVMsJson);
                        }
                        else
                        {
                            level.approvalLevelDepartmentVMs = new List<GetApprovalLevelDepartmentVM>();
                        }
                    }

                    response.isSuccess = true;
                    response.ResponseMessage = "Approval  levels fetched successfully.";
                    response.Data = result;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"Error fetching approval scheme levels: {ex.Message}";
            }
            return response;
        }
        public async Task<APIResponse> GetAllApprovalLevelsByApprovalMasterId(int ApprovalMasterId)
        {
            var response = new APIResponse();
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@ApprovalMasterId", ApprovalMasterId, DbType.Int32);

                    var result = await con.QueryAsync<GetApprovalLevelVM>(
                        "usp_GetAllApprovalLevelsByApprovalMasterId",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    // Deserialize the JSON string for approvalSchemeLevelDepartmentVMs
                    foreach (var level in result)
                    {
                        if (!string.IsNullOrEmpty(level.approvalLevelDepartmentVMsJson))
                        {
                            level.approvalLevelDepartmentVMs = JsonConvert.DeserializeObject<List<GetApprovalLevelDepartmentVM>>(level.approvalLevelDepartmentVMsJson);
                        }
                        else
                        {
                            level.approvalLevelDepartmentVMs = new List<GetApprovalLevelDepartmentVM>();
                        }
                    }

                    response.isSuccess = true;
                    response.ResponseMessage = "Approval scheme levels fetched successfully.";
                    response.Data = result;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = $"Error fetching approval scheme levels: {ex.Message}";
            }
            return response;
        }


        // For usp_AutomateProbationEndApprovalRequests
        public async Task<APIResponse> AutomateProbationEndApprovalRequests(int approvalMasterId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@ApprovalMasterId", approvalMasterId);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

                    await connection.ExecuteAsync(
                        "usp_AutomateProbationEndApprovalRequests",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    var success = parameters.Get<bool>("@Success");
                    var responseMessage = parameters.Get<string>("@ResponseMessage");

                    return new APIResponse
                    {
                        isSuccess = success,
                        ResponseMessage = responseMessage
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Error: {ex.Message}"
                };
            }
        }
        public async Task<APIResponse> AutomateLoanEndApprovalRequests(int approvalMasterId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@ApprovalMasterId", approvalMasterId);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);

                    await connection.ExecuteAsync(
                        "usp_AutomateLoanApprovalRequests",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    var success = parameters.Get<bool>("@Success");
                    var responseMessage = parameters.Get<string>("@ResponseMessage");

                    return new APIResponse
                    {
                        isSuccess = success,
                        ResponseMessage = responseMessage
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Error: {ex.Message}"
                };
            }
        }

        // For usp_EscalatePendingApprovalRequests
        public async Task<EscalationReturnPara> EscalatePendingApprovalRequests()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();

                    parameters.Add(
                        "@Success",
                        dbType: DbType.Boolean,
                        direction: ParameterDirection.Output
                    );

                    parameters.Add(
                        "@ResponseMessage",
                        dbType: DbType.String,
                        size: 1000,
                        direction: ParameterDirection.Output
                    );

                    // ⭐ IMPORTANT FIX
                    parameters.Add(
                        "@EscalatedData",
                        dbType: DbType.String,
                        size: -1, // NVARCHAR(MAX)
                        direction: ParameterDirection.Output
                    );

                    await connection.ExecuteAsync(
                        "usp_EscalatePendingApprovalRequests",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return new EscalationReturnPara
                    {
                        IsSuccess = parameters.Get<bool>("@Success"),
                        ResponseMessage = parameters.Get<string>("@ResponseMessage"),
                        EscalatedData = parameters.Get<string>("@EscalatedData")
                    };
                }
            }
            catch (Exception ex)
            {
                return new EscalationReturnPara
                {
                    IsSuccess = false,
                    ResponseMessage = $"Error: {ex.Message}"
                };
            }
        }

        // For sp_ApprovalRequestLevel_Action
        public async Task<ApprovalRequestLevelActionVm> ApprovalRequestLevelAction(ApprovalRequestLevelActionPara para)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@ApprovalRequestId", para.ApprovalRequestId);
                    parameters.Add("@ApprovalRequestLevelId", para.ApprovalRequestLevelId);
                    parameters.Add("@StatusId", para.StatusId);
                    parameters.Add("@ActionBy", para.ActionBy);
                    parameters.Add("@Remarks", para.Remarks);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                    parameters.Add("@IsAllLevelsCompleted", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync(
                        "sp_ApprovalRequestLevel_Action",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    // Nullable types use karein aur null checks lagayein
                    var success = parameters.Get<bool?>("@Success") ?? false;
                    var responseMessage = parameters.Get<string>("@ResponseMessage") ?? "No response message.";
                    var isAllLevelsCompleted = parameters.Get<bool?>("@IsAllLevelsCompleted") ?? false;

                    return new ApprovalRequestLevelActionVm
                    {
                        IsAllLevelsCompleted = isAllLevelsCompleted,
                        IsSuccess = success,
                        ResponseMessage = responseMessage
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApprovalRequestLevelActionVm
                {
                    IsAllLevelsCompleted = false,
                    IsSuccess = false,
                    ResponseMessage = $"Error: {ex.Message}"
                };
            }
        }
        public async Task<ApprovalRequestLevelActionVm> LoanApprovalRequestLevelAction(ApprovalRequestLevelActionPara para)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@ApprovalRequestId", para.ApprovalRequestId);
                    parameters.Add("@ApprovalRequestLevelId", para.ApprovalRequestLevelId);
                    parameters.Add("@StatusId", para.StatusId);
                    parameters.Add("@ActionBy", para.ActionBy);
                    parameters.Add("@Remarks", para.Remarks);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 500);
                    parameters.Add("@IsAllLevelsCompleted", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync(
                        "sp_LoanApprovalRequestLevel_Action",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    // Nullable types use karein aur null checks lagayein
                    var success = parameters.Get<bool?>("@Success") ?? false;
                    var responseMessage = parameters.Get<string>("@ResponseMessage") ?? "No response message.";
                    var isAllLevelsCompleted = parameters.Get<bool?>("@IsAllLevelsCompleted") ?? false;

                    return new ApprovalRequestLevelActionVm
                    {
                        IsAllLevelsCompleted = isAllLevelsCompleted,
                        IsSuccess = success,
                        ResponseMessage = responseMessage
                    };
                }
            }
            catch (Exception ex)
            {
                return new ApprovalRequestLevelActionVm
                {
                    IsAllLevelsCompleted = false,
                    IsSuccess = false,
                    ResponseMessage = $"Error: {ex.Message}"
                };
            }
        }


        public async Task<APIResponse> GetPendingApprovalRequests(GetPendingApprovalRequestsPara para)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@ApproverEmployeeId", para.ApproverEmployeeId);
                    parameters.Add("@StatusId", para.StatusId);

                    var pendingApprovalRequests = await connection.QueryAsync<dynamic>(
                        "usp_GetPendingApprovalRequests",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = "Pending approval requests retrieved successfully.",
                        Data = pendingApprovalRequests.AsList()
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<APIResponse> GetUpcomingProbationDetails(GetUpcomingProbationDetailsPara para)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@EmployeeId", para.EmployeeId);

                    var upcomingProbations = await connection.QueryAsync<dynamic>(
                        "GetAllUpCommingProbationDetails",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = "Upcoming probation details retrieved successfully.",
                        Data = upcomingProbations.AsList()
                    };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Error: {ex.Message}",
                    Data = null
                };
            }
        }

        
    }
}

