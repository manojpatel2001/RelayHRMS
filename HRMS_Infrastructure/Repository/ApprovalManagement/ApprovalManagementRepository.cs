using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.ApprovalManagement;
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

        // -------------------------------------------------------------
        // APPROVAL SCHEME LEVEL
        // -------------------------------------------------------------
        public async Task<APIResponse> ManageApprovalSchemeLevel(List<ApprovalSchemeLevelVM> models)
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
                            // Handle ApprovalSchemeLevel
                            var p = new DynamicParameters();
                            p.Add("@Action", "Upsert");
                            p.Add("@ApprovalSchemeLevelId", model.ApprovalSchemeLevelId);
                            p.Add("@SchemeId", model.SchemeId);
                            p.Add("@SequenceNo", model.SequenceNo);
                            p.Add("@ApproverEmployeeId", model.ApproverEmployeeId);
                            p.Add("@ApproverDesignationName", model.ApproverDesignationName);
                            p.Add("@IsDepartmentBased", model.IsDepartmentBased);
                            p.Add("@EscalationDays", model.EscalationDays);
                            p.Add("@SkipDays", model.SkipDays);
                            p.Add("@IsNotMandatory", model.IsNotMandatory);
                            p.Add("@CreatedBy", model.CreatedBy);
                            p.Add("@UpdatedBy", model.UpdatedBy);
                            p.Add("@DeletedBy", model.DeletedBy);
                            p.Add("@CompanyId", model.CompanyId);
                            p.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                            p.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 300);
                            p.Add("@ReturnId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                            await con.ExecuteAsync(
                                "sp_Manage_ApprovalSchemeLevel",
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

                            // Handle ApprovalSchemeLevelDepartment if IsDepartmentBased is true
                            if (model.IsDepartmentBased && model.approvalSchemeLevelDepartmentVMs != null && model.approvalSchemeLevelDepartmentVMs.Any())
                            {
                                foreach (var dept in model.approvalSchemeLevelDepartmentVMs)
                                {
                                    var deptParams = new DynamicParameters();
                                    deptParams.Add("@ApprovalSchemeLevelId", returnId);
                                    deptParams.Add("@DepartmentId", dept.DepartmentId);
                                    deptParams.Add("@ApproverEmployeeId", dept.DeptApproverEmployeeId);
                                    deptParams.Add("@ApproverDesignationName", dept.DeptApproverDesignationName);
                                    deptParams.Add("@IsActive", true);
                                    deptParams.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 100);
                                    deptParams.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);

                                    await con.ExecuteAsync(
                                        "usp_UpsertApprovalSchemeLevelDepartment",
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

        public async Task<APIResponse> DeleteApprovalSchemeLevel(ApprovalSchemeLevelPara para)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@Action", "Delete");
                    parameters.Add("@SchemeId", para.SchemeId);
                    parameters.Add("@ApprovalSchemeLevelId", para.ApprovalSchemeLevelId);
                    parameters.Add("@DeletedBy", para.DeletedBy);
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@Message", dbType: DbType.String, direction: ParameterDirection.Output, size: 300);
                    parameters.Add("@ReturnId", dbType: DbType.Int32, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync(
                        "sp_Manage_ApprovalSchemeLevel",
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


        // -------------------------------------------------------------
        // OPTIONAL GET METHODS
        // -------------------------------------------------------------
        public async Task<IEnumerable<dynamic>> GetApprovalSchemeLevels()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                return await con.QueryAsync<dynamic>("SELECT * FROM ApprovalSchemeLevel WHERE IsActive = 1");
            }
        }

        public async Task<IEnumerable<dynamic>> GetApprovalRequests()
        {
            using (var con = new SqlConnection(_connectionString))
            {
                return await con.QueryAsync<dynamic>("SELECT * FROM ApprovalRequest WHERE IsActive = 1");
            }
        }

        public async Task<IEnumerable<dynamic>> GetApprovalRequestLevels(int requestId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                return await con.QueryAsync<dynamic>(
                    "SELECT * FROM ApprovalRequestLevel WHERE ApprovalRequestId = @requestId AND IsActive = 1",
                    new { requestId });
            }
        }

        public async Task<IEnumerable<dynamic>> GetApprovalRequestHistory(int requestId)
        {
            using (var con = new SqlConnection(_connectionString))
            {
                return await con.QueryAsync<dynamic>(
                    "SELECT * FROM ApprovalRequestHistory WHERE ApprovalRequestId = @requestId AND IsActive = 1",
                    new { requestId });
            }
        }

        public Task<APIResponse> ManageApprovalSchemeLevelDepartment(ApprovalSchemeLevelDepartmentVM model)
        {
            throw new NotImplementedException();
        }

        public async Task<APIResponse> GetAllApprovalSchemeLevelsByCompanyId(int companyId)
        {
            var response = new APIResponse();
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompanyId", companyId, DbType.Int32);

                    var result = await con.QueryAsync<GetApprovalSchemeLevelVM>(
                        "usp_GetAllApprovalSchemeLevelsByCompanyId",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    // Deserialize the JSON string for approvalSchemeLevelDepartmentVMs
                    foreach (var level in result)
                    {
                        if (!string.IsNullOrEmpty(level.approvalSchemeLevelDepartmentVMsJson))
                        {
                            level.approvalSchemeLevelDepartmentVMs = JsonConvert.DeserializeObject<List<GetApprovalSchemeLevelDepartmentVM>>(level.approvalSchemeLevelDepartmentVMsJson);
                        }
                        else
                        {
                            level.approvalSchemeLevelDepartmentVMs = new List<GetApprovalSchemeLevelDepartmentVM>();
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
        public async Task<APIResponse> GetAllApprovalSchemeLevelsBySchemeId(int schemeId)
        {
            var response = new APIResponse();
            try
            {
                using (var con = new SqlConnection(_connectionString))
                {
                    await con.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@SchemeId", schemeId, DbType.Int32);

                    var result = await con.QueryAsync<GetApprovalSchemeLevelVM>(
                        "usp_GetAllApprovalSchemeLevelsBySchemeId",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    // Deserialize the JSON string for approvalSchemeLevelDepartmentVMs
                    foreach (var level in result)
                    {
                        if (!string.IsNullOrEmpty(level.approvalSchemeLevelDepartmentVMsJson))
                        {
                            level.approvalSchemeLevelDepartmentVMs = JsonConvert.DeserializeObject<List<GetApprovalSchemeLevelDepartmentVM>>(level.approvalSchemeLevelDepartmentVMsJson);
                        }
                        else
                        {
                            level.approvalSchemeLevelDepartmentVMs = new List<GetApprovalSchemeLevelDepartmentVM>();
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
    }
}
