using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.Master.Scheme;
using HRMS_Core.VM;
using HRMS_Core.VM.ApprovalManagement;
using HRMS_Core.VM.Leave;
using HRMS_Core.VM.OtherMaster;
using HRMS_Core.VM.Report;
using HRMS_Core.VM.Salary;
using HRMS_Core.VM.UpdateEmployee;
using HRMS_Infrastructure.Interface.OtherMaster;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace HRMS_Infrastructure.Repository.OtherMaster
{
    public class ManpowerRequisitionRepository : IManpowerRequisitionRepository
    {
        private HRMSDbContext _db;
        private readonly string _connectionString;

        public ManpowerRequisitionRepository(HRMSDbContext db) 
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

   
    public async Task<APIResponse> GetAllManpowerRequisitions(CommonParameter commonParameter)
    {
        var response = new APIResponse();
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CompanyId", commonParameter.CompanyId);
                parameters.Add("@BranchId", commonParameter.BranchId);

                // Execute the stored procedure and map results to a dynamic list
                var result = await connection.QueryAsync<dynamic>(
                    "GetAllManpowerRequisitions",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (!result.AsList().Any())
                {
                    response.isSuccess = false;
                    response.ResponseMessage = "No records found.";
                    response.Data = new List<dynamic>(); 
                    return response;
                }

                response.isSuccess = true;
                response.ResponseMessage = "Success!";
                response.Data = result.AsList(); 
            }
        }
        catch (Exception ex)
        {
            response.isSuccess = false;
            response.ResponseMessage = ex.Message;
            response.Data = new List<dynamic>(); 
        }
        return response;
        }
        public async Task<APIResponse> CreateManpowerRequisition(ManpowerRequisition manpowerRequisition)
        {
            try
            {
                using var command = _db.Database.GetDbConnection().CreateCommand();
                command.CommandText = "ManageManpowerRequisition";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("@Action", "CREATE"));
                command.Parameters.Add(new SqlParameter("@DepartmentId", manpowerRequisition.DepartmentId));
                command.Parameters.Add(new SqlParameter("@RequirementType", manpowerRequisition.RequirementType));
                command.Parameters.Add(new SqlParameter("@EmployeeName", manpowerRequisition.EmployeeName ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@PersonalEmail", manpowerRequisition.PersonalEmail ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@ContactNumber", manpowerRequisition.ContactNumber ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@ClosureBy", manpowerRequisition.ClosureBy ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@DesignationId", manpowerRequisition.DesignationId));
                command.Parameters.Add(new SqlParameter("@ExperienceRange", manpowerRequisition.ExperienceRange ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@EducationalQualification", manpowerRequisition.EducationalQualification ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@ComputerSkills", manpowerRequisition.ComputerSkills ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@JobResponsibility", manpowerRequisition.JobResponsibility ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@Age", manpowerRequisition.Age));
                command.Parameters.Add(new SqlParameter("@Gender", manpowerRequisition.Gender ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@OtherBenefits", manpowerRequisition.OtherBenefits ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@SystemRequire", manpowerRequisition.SystemRequire ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@EmailIdRequire", manpowerRequisition.EmailIdRequire ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@SIMRequire", manpowerRequisition.Simrequire ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@ERP_ID", manpowerRequisition.ErpId ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@ReportingToId", manpowerRequisition.ReportingToId));
                command.Parameters.Add(new SqlParameter("@DateOfJoining", manpowerRequisition.DateOfJoining));
                command.Parameters.Add(new SqlParameter("@CategoryOfEmployment", manpowerRequisition.CategoryOfEmployment ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@TakeHomeSalary", manpowerRequisition.TakeHomeSalary));
                command.Parameters.Add(new SqlParameter("@CreatedBy", manpowerRequisition.CreatedBy ?? (object)DBNull.Value));
                command.Parameters.Add(new SqlParameter("@CompanyId", manpowerRequisition.CompanyId));
                command.Parameters.Add(new SqlParameter("@DateOfBirth", manpowerRequisition.DateOfBirth));
                command.Parameters.Add(new SqlParameter("@Amount", manpowerRequisition.Amount));
                command.Parameters.Add(new SqlParameter("@NumberOfPosition", manpowerRequisition.NumberOfPosition));
                command.Parameters.Add(new SqlParameter("@JobCategory", SqlDbType.Int)
                {
                    Value = (object?)manpowerRequisition.JobCategory ?? DBNull.Value
                });
                // ✅ NEW
                command.Parameters.Add(new SqlParameter("@BranchId", SqlDbType.Int)
                {
                    Value = (object?)manpowerRequisition.BranchId ?? DBNull.Value
                });
                // ✅ NEW
                command.Parameters.Add(new SqlParameter("@CustomerName", SqlDbType.NVarChar, 200)
                {
                    Value = (object?)manpowerRequisition.CustomerName ?? DBNull.Value
                });

                await _db.Database.OpenConnectionAsync();

                using var reader = await command.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    int success = reader.GetInt32(reader.GetOrdinal("Success"));
                    string responseMessage = reader["ResponseMessage"].ToString();

                    if (success == 1)
                    {
                        int manpowerRequisitionId = reader.GetInt32(reader.GetOrdinal("ManpowerRequisitionId"));
                        string serialNo = reader["SerialNo"].ToString();

                        return new APIResponse
                        {
                            isSuccess = true,
                            ResponseMessage = responseMessage,
                            Data = new ManpowerRequisitionCreatedData
                            {
                                ManpowerRequisitionId = manpowerRequisitionId,
                                SerialNo = serialNo
                            }
                        };
                    }
                    else
                    {
                        return new APIResponse
                        {
                            isSuccess = false,
                            ResponseMessage = responseMessage,
                            Data = null
                        };
                    }
                }

                return new APIResponse { isSuccess = false, ResponseMessage = "No response from database.", Data = null };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = $"Error: {ex.Message}", Data = null };
            }
            finally
            {
                await _db.Database.CloseConnectionAsync();
            }
        }
        public async Task<APIResponse> UpdateManpowerRequisition(ManpowerRequisition manpowerRequisition)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
        EXEC ManageManpowerRequisition
            @Action = {"UPDATE"},
            @ManpowerRequisitionId = {manpowerRequisition.ManpowerRequisitionId},
            @DepartmentId = {manpowerRequisition.DepartmentId},
            @RequirementType = {manpowerRequisition.RequirementType},
            @EmployeeName = {manpowerRequisition.EmployeeName},
            @PersonalEmail = {manpowerRequisition.PersonalEmail},
            @ContactNumber = {manpowerRequisition.ContactNumber},
            @ClosureBy = {manpowerRequisition.ClosureBy},
            @DesignationId = {manpowerRequisition.DesignationId},
            @ExperienceRange = {manpowerRequisition.ExperienceRange},
            @EducationalQualification = {manpowerRequisition.EducationalQualification},
            @ComputerSkills = {manpowerRequisition.ComputerSkills},
            @JobResponsibility = {manpowerRequisition.JobResponsibility},
            @Age = {manpowerRequisition.Age},
            @Gender = {manpowerRequisition.Gender},
            @OtherBenefits = {manpowerRequisition.OtherBenefits},
            @SystemRequire = {manpowerRequisition.SystemRequire},
            @EmailIdRequire = {manpowerRequisition.EmailIdRequire},
            @SIMRequire = {manpowerRequisition.Simrequire},
            @ERP_ID = {manpowerRequisition.ErpId},
            @ReportingToId = {manpowerRequisition.ReportingToId},
            @DateOfJoining = {manpowerRequisition.DateOfJoining},
            @CategoryOfEmployment = {manpowerRequisition.CategoryOfEmployment},
            @TakeHomeSalary = {manpowerRequisition.TakeHomeSalary},
            @IsEnabled = {manpowerRequisition.IsEnabled},
            @IsDeleted = {manpowerRequisition.IsDeleted},
            @UpdatedBy = {manpowerRequisition.UpdatedBy},
            @DateOfBirth = {manpowerRequisition.DateOfBirth},
            @Amount = {manpowerRequisition.Amount},
            @NumberOfPosition = {manpowerRequisition.NumberOfPosition},
            @JobCategory = {manpowerRequisition.JobCategory},
            @BranchId = {manpowerRequisition.BranchId},
            @CustomerName = {manpowerRequisition.CustomerName}
        ")
                    .ToListAsync();

                var spResult = result.FirstOrDefault();

                if (spResult == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Failed to update requisition.", Data = null };

                return new APIResponse
                {
                    isSuccess = spResult.Success == 1,
                    ResponseMessage = spResult.ResponseMessage,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = $"Error: {ex.Message}", Data = null };
            }
        }
        public async Task<APIResponse> DeleteManpowerRequisition(DeleteRecordVM model)
        {
            try
            {
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
            EXEC ManageManpowerRequisition
                @Action = {"DELETE"},
                @ManpowerRequisitionId = {model.Id},
                @UpdatedBy = {model.DeletedBy}
            ")
                    .ToListAsync();

                var spResult = result.FirstOrDefault();

                if (spResult == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Failed to delete requisition.", Data = null };

                return new APIResponse
                {
                    isSuccess = spResult.Success == 1,
                    ResponseMessage = spResult.ResponseMessage,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = $"Error: {ex.Message}", Data = null };
            }
        }



        public async Task<APIResponse> GetDropDownForManpower(int CompanyId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    using (var multi = await connection.QueryMultipleAsync(
                        "GetDropDownForManpower",
                        new { CompanyId },
                        commandType: CommandType.StoredProcedure))
                    {
                        var result = new VMGetDropDownForManpower
                        {
                            ReportingEmployees = (await multi.ReadAsync<ReportingEmployeeViewModel>()).AsList(),

                           Designations = (await multi.ReadAsync<HRMS_Core.VM.UpdateEmployee.DesignationViewModel>()).AsList(),
                            Departments = (await multi.ReadAsync<HRMS_Core.VM.UpdateEmployee.DepartmentViewModel>()).AsList(),
                            Branches = (await multi.ReadAsync<branchViewModel>()).AsList()
                        };



                        return new APIResponse { Data = result, ResponseMessage = "Fetched successfully!", isSuccess = true };
                    }
                }
            }
            catch (Exception)
            {
                return new APIResponse { ResponseMessage = "Some thing Went wrong!", isSuccess = false };
            }
        }


        public async Task<APIResponse> GetManpowerRequisitionByManpowerRequisitionId(int ManpowerRequisitionId)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@ManpowerRequisitionId", ManpowerRequisitionId);

                    // Execute the stored procedure and map the result to a dynamic object
                    var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                        "GetManpowerRequisitionByManpowerRequisitionId",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result != null)
                    {
                        response.isSuccess = true;
                        response.Data = result;
                        response.ResponseMessage = "Fetch successfully!";
                    }
                    else
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "No Record found!";
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                response.isSuccess = false;
                response.ResponseMessage = "Something went wrong!";
                response.Data = null;
            }
            return response;
        }
        public async Task<APIResponse> GetAllSerialNo(CommonParameter commonParameter)
        {
            try
            {

                var companyIdParam = new SqlParameter("@CompanyId", commonParameter.CompanyId);
                var empIdParam = new SqlParameter("@EmpId", commonParameter.EmployeeId);

                var data = await _db.Set<SerialNoViewModel>()
                    .FromSqlRaw("EXEC GetAllSerialNo @CompanyId, @EmpId", companyIdParam, empIdParam)
                    .ToListAsync();

                if (data == null || !data.Any())
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "No record found!" };
                }

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Fetch successfully!" };
            }
            catch (Exception ex)
            {
                // Log exception details
                Console.WriteLine($"Error in GetAllSerialNo: {ex}");
                return new APIResponse { isSuccess = false, ResponseMessage = "Something went wrong!" };
            }
        }

        public async Task<APIResponse> UpdateJoinningDetails(UpdateJoinningDetailsModel model)
        {
            try
            {
                //UPDATE_EmployeeInfo,UPDATE_JoiningDetails,UPDATE_SalaryDetails,UPDATE_ContactDetails,UPDATE_DocumentDetails
                var result = await _db.Set<SP_Response>()
                    .FromSqlInterpolated($@"
                    EXEC UpdateJoinningDetails
                        @Action = {model.Action},
                        @ManpowerRequisitionId = {model.ManpowerRequisitionId},
                        @OperationType = {model.OperationType},
                        @PresentAddress = {model.PresentAddress},
                        @PermanentAddress = {model.PermanentAddress},
                        @MaritalStatus = {model.MaritalStatus},
                        @DateOfBirth = {model.DateOfBirth},
                        @BloodGroup = {model.BloodGroup},
                       
                        @InterviewedBy = {model.InterviewedBy},
                        @InterviewDate = {model.InterviewDate},
                        @InterviewPlace = {model.InterviewPlace},
                        @ClientName = {model.ClientName},
                        @ERPCode = {model.ERPCode},
                        @PreviousCompanyUAN = {model.PreviousCompanyUAN},
                        @PreviousCompanyESIC = {model.PreviousCompanyESIC},
                       
                        @PFUAN = {model.PFUAN},
                        @ESICNo = {model.ESICNo},
                        @PAN = {model.PAN},
                        @GrossSalary = {model.GrossSalary},
                        @NetSalary = {model.NetSalary},
                        
                        @ContactNo1 = {model.ContactNo1},
                        @ContactPersonName1 = {model.ContactPersonName1},
                        @ContactPersonRelation1 = {model.ContactPersonRelation1},
                        @ContactNo2 = {model.ContactNo2},
                        @ContactPersonName2 = {model.ContactPersonName2},
                        @ContactPersonRelation2 = {model.ContactPersonRelation2},
                        @ContactNo3 = {model.ContactNo3},
                        @ContactPersonName3 = {model.ContactPersonName3},
                        @ContactPersonRelation3 = {model.ContactPersonRelation3},
                        
                        @AadharCardNo = {model.AadharCardNo},
                        @AadharCardCopyPath = {model.AadharCardCopyPath},
                        @PanCardNo = {model.PanCardNo},
                        @PanCardCopyPath = {model.PanCardCopyPath},
                        @FreshResumePath = {model.FreshResumePath},
                        @PassportSizePhotoCopyPath = {model.PassportSizePhotoCopyPath},
                        @CancelledChequePath = {model.CancelledChequePath},
                        @PayslipPath = {model.PayslipPath},
                        @ExperienceCertificatePath = {model.ExperienceCertificatePath}
                ")
                    .ToListAsync();
                var data = result.FirstOrDefault() ?? null;
                if (data != null && data.Success>0)
                {
                    return new APIResponse { isSuccess = true, Data = data, ResponseMessage = data.ResponseMessage };
                }
                else
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = data.ResponseMessage??"Some thing went wrong!" };
                }
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Some thing went wrong!" };
            }
        }


        public async Task<APIResponse> GetAllJoiningManpowerRequisitions(CommonParameter commonParameter)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompanyId", commonParameter.CompanyId);
                    parameters.Add("@BranchId", commonParameter.BranchId);
                    parameters.Add("@StartDate", commonParameter.StartDate);
                    parameters.Add("@EndDate", commonParameter.EndDate);

                    // Execute the stored procedure and map results to ManpowerRequisitionModel
                    var result = await connection.QueryAsync<GetAllJoiningManpowerRequisitions>(
                        "GetAllJoiningManpowerRequisitions",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (!result.Any())
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "No records found.";
                        return response;
                    }

                    response.isSuccess = true;
                    response.ResponseMessage = "Success!";
                    response.Data = result.ToList();
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<SP_Response> ApprovalManPower(ManPowerfilter model)
        {
            try
            {
                var result = await _db.Set<SP_Response>().FromSqlInterpolated($@"
                EXEC usp_UpdateManpower
                 @ManpowerRequisitionId ={model.ManpowerRequisitionId},
                    @UpdatedBy = {model.@UpdatedBy},
                    @Status = {model.Status}
            ").ToListAsync();

                return result.FirstOrDefault() ?? new SP_Response { Success = 0, ResponseMessage = "Some thing went wrong!" };
            }
            catch
            {
                return new SP_Response { Success = -1, ResponseMessage = "Some thing went wrong!" };
            }
        }

        public async Task<APIResponse> GetAllManpowerRequisitionsAdmin(CommonParameter commonParameter)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompanyId", commonParameter.CompanyId);
           
                    // Execute the stored procedure and map results to a dynamic list
                    var result = await connection.QueryAsync<dynamic>(
                        "GetAllManpowerRequisitionsAdmin",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (!result.AsList().Any())
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "No records found.";
                        response.Data = new List<dynamic>();
                        return response;
                    }

                    response.isSuccess = true;
                    response.ResponseMessage = "Success!";
                    response.Data = result.AsList();
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
                response.Data = new List<dynamic>();
            }
            return response;
        }

        public async Task<APIResponse> GetManpowerRequisitionEmailDetails(int? ManpowerRequisitionId)
        {

            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@ManpowerRequisitionId", ManpowerRequisitionId);

                    // Execute the stored procedure and map the result to a dynamic object
                    var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                        "GetManpowerRequisitionEmailDetails",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result != null)
                    {
                        response.isSuccess = true;
                        response.Data = result;
                        response.ResponseMessage = "Fetch successfully!";
                    }
                    else
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "No Record found!";
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                response.isSuccess = false;
                response.ResponseMessage = "Something went wrong!";
                response.Data = null;
            }
            return response;
        }

        public async Task<APIResponse> GetAllManpowerRequisitionsEss(SearchVmCompOff model)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompanyId", model.CompId);
                    parameters.Add("@BranchId", model.BranchId ?? 0);
                    parameters.Add("@Status", string.IsNullOrWhiteSpace(model.Status) ? null : model.Status);
                    parameters.Add("@SearchBy", string.IsNullOrWhiteSpace(model.SearchType) ? null : model.SearchType);
                    parameters.Add("@SearchFor", string.IsNullOrWhiteSpace(model.SearchFor) ? null : model.SearchFor);

                    var result = await connection.QueryAsync<dynamic>(
                        "GetAllManpowerRequisitionsEss",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    var list = result.AsList();

                    if (!list.Any())
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "No records found.";
                        response.Data = new List<dynamic>();
                        return response;
                    }

                    response.isSuccess = true;
                    response.ResponseMessage = "Success!";
                    response.Data = list;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
                response.Data = new List<dynamic>();
            }
            return response;
        }

        public async Task<APIResponse> GetAllJoingWithApprovalCheck(SearchVmCompOff model)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompanyId", model.CompId);
                    parameters.Add("@EmployeeId", model.Emplooyeid ?? 0);
                    parameters.Add("@Status", string.IsNullOrWhiteSpace(model.Status) ? null : model.Status);
                    parameters.Add("@SearchBy", string.IsNullOrWhiteSpace(model.SearchType) ? null : model.SearchType);
                    parameters.Add("@SearchFor", string.IsNullOrWhiteSpace(model.SearchFor) ? null : model.SearchFor);

                    var result = await connection.QueryAsync<dynamic>(
                        "GetAllJoingWithApprovalCheck",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    var list = result.AsList();

                    if (!list.Any())
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "No records found.";
                        response.Data = new List<dynamic>();
                        return response;
                    }

                    response.isSuccess = true;
                    response.ResponseMessage = "Success!";
                    response.Data = list;
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
                response.Data = new List<dynamic>();
            }
            return response;
        }

        public async Task<APIResponse> sp_GetActiveEmployee(int Employeeid)
        {

            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@EmployeeId", Employeeid);

                    // Execute the stored procedure and map the result to a dynamic object
                    var result = await connection.QueryFirstOrDefaultAsync<dynamic>(
                        "sp_GetActiveEmployee",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (result != null)
                    {
                        response.isSuccess = true;
                        response.Data = result;
                        response.ResponseMessage = "Fetch successfully!";
                    }
                    else
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "No Record found!";
                    }
                }
            }
            catch (Exception ex)
            {
                // Log exception here if needed
                response.isSuccess = false;
                response.ResponseMessage = "Something went wrong!";
                response.Data = null;
            }
            return response;
        }

        public async Task<APIResponse> GetAllJoiningWithApprovalCheck_Admin(CommonParameter commonParameter)
        {
            var response = new APIResponse();
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@CompanyId", commonParameter.CompanyId);

                    // Execute the stored procedure and map results to a dynamic list
                    var result = await connection.QueryAsync<dynamic>(
                        "GetAllJoiningWithApprovalCheck_Admin",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    if (!result.AsList().Any())
                    {
                        response.isSuccess = false;
                        response.ResponseMessage = "No records found.";
                        response.Data = new List<dynamic>();
                        return response;
                    }

                    response.isSuccess = true;
                    response.ResponseMessage = "Success!";
                    response.Data = result.AsList();
                }
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
                response.Data = new List<dynamic>();
            }
            return response;
        }

        public async Task<APIResponse> GetManpowerRequisitionApprovalStatus(ManpowerApprovalStatusRequestDto request)
        {
            var response = new APIResponse();

            try
            {
                using var connection = new SqlConnection(_connectionString);

                var parameters = new DynamicParameters();
                parameters.Add("@ApprovalMasterId", request.ApprovalMasterId, DbType.Int32);
                parameters.Add("@ManpowerRequisitionId", request.ManpowerRequisitionId, DbType.Int32);
                // OUTPUT params
                parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                parameters.Add("@ResponseMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: -1);

                // ── Execute SP — reads 3 result sets ──
                using var multi = await connection.QueryMultipleAsync(
                    "usp_GetManpowerRequisitionApprovalStatus",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                // Result Set 1 — Header rows
                var headers = (await multi.ReadAsync<ManpowerApprovalStatusDto>()).ToList();

                // Result Set 2 — Level rows
                var levels = (await multi.ReadAsync<ApprovalLevelStatusDto>()).ToList();

                // Result Set 3 — History rows
                var history = (await multi.ReadAsync<ApprovalHistoryDto>()).ToList();

                // Read OUTPUT params
                bool spSuccess = parameters.Get<bool>("@Success");
                string spMessage = parameters.Get<string>("@ResponseMessage") ?? string.Empty;

                if (!spSuccess || headers.Count == 0)
                {
                    response.isSuccess = false;
                    response.ResponseMessage = spMessage.Length > 0 ? spMessage : "No records found.";
                    return response;
                }

                // ── Join levels and history into each header ──
                foreach (var header in headers)
                {
                    header.Levels = levels
                        .Where(l => l.ManpowerRequisitionId == header.ManpowerRequisitionId)
                        .OrderBy(l => l.LevelNo)
                        .ToList();

                    header.History = history
                        .Where(h => h.ManpowerRequisitionId == header.ManpowerRequisitionId)
                        .OrderBy(h => h.ActionDate)
                        .ToList();
                }

                response.isSuccess = true;
                response.ResponseMessage = spMessage.Length > 0 ? spMessage : "Success";
                response.Data = headers;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
                response.Data = null;
            }

            return response;
        }
    }

}
