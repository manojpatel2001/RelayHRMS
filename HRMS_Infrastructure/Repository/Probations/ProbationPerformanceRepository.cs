using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Probations;
using HRMS_Core.VM;
using HRMS_Core.VM.ApprovalManagement;
using HRMS_Core.VM.Probations;
using HRMS_Infrastructure.Interface.Probations;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Probations
{
    public class ProbationPerformanceRepository : IProbationPerformanceRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;


        public ProbationPerformanceRepository(HRMSDbContext db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;

        }

        public async Task<APIResponse> GetAllProbationStatus()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var result = await connection.QueryAsync<dynamic>(
                        sql: "GetAllProbationStatus",
                        commandType: CommandType.StoredProcedure);

                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = "Probation statuses retrieved successfully.",
                        Data = result
                    };
                }
            }
            catch (SqlException sqlEx)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Database error: {sqlEx.Message}",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Error retrieving probation statuses: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<APIResponse> GetAllProbationEvaluationPeriods(int? probationStatusId = null)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ProbationStatusId", probationStatusId, DbType.Int32, ParameterDirection.Input);

                    var result = await connection.QueryAsync<dynamic>(
                        sql: "GetAllProbationEvaluationPeriod",
                        param: dynamicParameters,
                        commandType: CommandType.StoredProcedure);

                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = "Probation evaluation periods retrieved successfully.",
                        Data = result
                    };
                }
            }
            catch (SqlException sqlEx)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Database error: {sqlEx.Message}",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Error retrieving probation evaluation periods: {ex.Message}",
                    Data = null
                };
            }
        }

        //remove
        public async Task<List<ProbationEmployeeVM>> GetAllProbationEmployees(int ProbationManagerId)
        {
            try
            {
                return await _db.Set<ProbationEmployeeVM>()
                    .FromSqlInterpolated($"EXEC GetAllProbationEmployees @ProbationManagerId = {ProbationManagerId}")
                    .ToListAsync();
            }
            catch
            {
                return new List<ProbationEmployeeVM>();
            }
        }

        public async Task<EmployeeProbationDetailVM?> GetEmployeeForProbationByEmployeeId(int EmployeeId)
        {
            try
            {
                var result = await _db.Set<EmployeeProbationDetailVM>()
                    .FromSqlInterpolated($"EXEC GetEmployeeForProbationByEmployeeId @EmployeeId = {EmployeeId}")
                    .ToListAsync();
                return result.FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task<APIResponse> CreateProbationPerformance(ProbationPerformance probationPerformance)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var parameters = new DynamicParameters();

                    // Input parameters
                    parameters.Add("@ProbationPerformanceId", probationPerformance.ProbationPerformanceId);
                    parameters.Add("@EmployeeId", probationPerformance.EmployeeId);
                    parameters.Add("@ProbationStatusId", probationPerformance.ProbationStatusId);
                    parameters.Add("@ProbationEvaluationPeriodId", probationPerformance.ProbationEvaluationPeriodId);
                    parameters.Add("@Rating", probationPerformance.Rating);
                    parameters.Add("@ProbationEvaluationDate", probationPerformance.ProbationEvaluationDate);
                    parameters.Add("@RemarksOfApprover", probationPerformance.RemarksOfApprover);
                    parameters.Add("@EmployeeTypeId", probationPerformance.EmployeeTypeId);
                    parameters.Add("@CreatedBy", probationPerformance.CreatedBy);
                    parameters.Add("@Level", probationPerformance.Level);

                    // OUTPUT PARAMETERS
                    parameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@ResponseMessage", dbType: DbType.String, size: 200, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync(
                        "ManageProbationPerformance",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return new APIResponse
                    {
                        isSuccess = parameters.Get<bool>("@Success"),
                        ResponseMessage = parameters.Get<string>("@ResponseMessage")
                    };
                }
            }
            catch (SqlException sqlEx)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "SQL Error: " + sqlEx.Message
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "Error: " + ex.Message
                };
            }
        }


        public async Task<APIResponse> GetPendingApprovalRequestsWithHistory(GetPendingApprovalRequestsWithHistoryPara parameters)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ApproverEmployeeId", parameters.ApproverEmployeeId, DbType.Int32);
                    dynamicParameters.Add("@StatusId", parameters.StatusId, DbType.String);

                    var results = await connection.QueryAsync<PendingApprovalRequestwithHistrory>(sql: "usp_GetPendingApprovalRequestsWithHistory",
                        param: dynamicParameters,
                        commandType: CommandType.StoredProcedure);

                    // Parse JSON fields if needed
                    foreach (var result in results)
                    {
                        if (!string.IsNullOrEmpty(result.PreviousApprovalLevelsJson))
                        {
                            result.PreviousApprovalLevels = JsonConvert.DeserializeObject<List<ApprovalLevelHistory>>(result.PreviousApprovalLevelsJson);
                        }

                       
                    }

                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = "Pending approval requests retrieved successfully",
                        Data = results
                    };
                }
            }
            catch (SqlException sqlEx)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Database error: {sqlEx.Message}",
                    Data = null
                };
            }
            catch (JsonException jsonEx)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"JSON parsing error: {jsonEx.Message}",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Error retrieving pending approval requests: {ex.Message}",
                    Data = null
                };
            }
        }
        public async Task<APIResponse> GetPendingApprovalRequestsWithHistory1(GetPendingApprovalRequestsWithHistoryPara1 parameters)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ApproverEmployeeId", parameters.ApproverEmployeeId, DbType.Int32);
                    dynamicParameters.Add("@StatusId", parameters.StatusId, DbType.String);
                    dynamicParameters.Add("@ApprovalMasterId", parameters.ApprovalMasterId, DbType.Int32);

                    var results = await connection.QueryAsync<PendingApprovalRequestwithHistrory1>(sql: "usp_GetPendingApprovalRequestsWithHistory1",
                        param: dynamicParameters,
                        commandType: CommandType.StoredProcedure);

                    // Parse JSON fields if needed
                    foreach (var result in results)
                    {
                        if (!string.IsNullOrEmpty(result.PreviousApprovalLevelsJson))
                        {
                            result.PreviousApprovalLevels = JsonConvert.DeserializeObject<List<ApprovalLevelHistory>>(result.PreviousApprovalLevelsJson);
                        }

                       
                    }

                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = "Pending approval requests retrieved successfully",
                        Data = results
                    };
                }
            }
            catch (SqlException sqlEx)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Database error: {sqlEx.Message}",
                    Data = null
                };
            }
            catch (JsonException jsonEx)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"JSON parsing error: {jsonEx.Message}",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Error retrieving pending approval requests: {ex.Message}",
                    Data = null
                };
            }
        }

        //remove
        public async Task<ApproverDetailsViewModel?> GetApproverDetails(int ApprovalRequestId)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();
                    var parameters = new DynamicParameters();
                    parameters.Add("@ApprovalRequestId", ApprovalRequestId, DbType.Int32);

                    var result= await connection.QueryFirstOrDefaultAsync<ApproverDetailsViewModel>(
                        "GetApprovalRequestLevels",
                        parameters,
                        commandType: CommandType.StoredProcedure
                    );
                   
                    
                    return result;
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }


        public async Task<List<ConfirmationProbationDetails>> GetAllConfirmationProbationDetails(GetAllConfirmationProbationDetailsPara parameters)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@CompanyId", parameters.CompanyId, DbType.Int32);
                    dynamicParameters.Add("@EmployeeId", parameters.EmployeeId, DbType.Int32);
                    dynamicParameters.Add("@StatusId", parameters.StatusId, DbType.Int32);

                    var results = await connection.QueryAsync<ConfirmationProbationDetails>(
                        sql: "GetAllConfirmationProbationDetails",
                        param: dynamicParameters,
                        commandType: CommandType.StoredProcedure);

                    return results.ToList(); // ← Missing semicolon fixed
                }
            }
            catch (SqlException sqlEx)
            {
                // Log error if needed
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                return new List<ConfirmationProbationDetails>(); // return empty list
            }
            catch (Exception ex)
            {
                // Log error
                Console.WriteLine($"Error: {ex.Message}");
                return new List<ConfirmationProbationDetails>(); // return empty list
            }
        }


        public async Task<APIResponse> UpdateMailRequest(int approvalRequestId, bool isMailSent)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    var dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("@ApprovalRequestId", approvalRequestId, DbType.Int32);
                    dynamicParameters.Add("@IsMailSent", isMailSent, DbType.Boolean);
                    dynamicParameters.Add("@Success", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    dynamicParameters.Add("@ResponseMessage", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync(
                        sql: "UpdateMailRequest",
                        param: dynamicParameters,
                        commandType: CommandType.StoredProcedure);

                    // Get the output parameter values
                    bool success = dynamicParameters.Get<bool>("@Success");
                    string responseMessage = dynamicParameters.Get<string>("@ResponseMessage");

                    return new APIResponse
                    {
                        isSuccess = success,
                        ResponseMessage = responseMessage,
                        Data = null
                    };
                }
            }
            catch (SqlException sqlEx)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Database error: {sqlEx.Message}",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = $"Error updating mail request: {ex.Message}",
                    Data = null
                };
            }
        }


    }
}
