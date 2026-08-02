using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Recruitment;
using HRMS_Infrastructure.Interface.Recruitment;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Recruitment
{
    public class CandidateEmployeeConversionRepository : ICandidateEmployeeConversionRepository
    {
        private readonly string _connectionString;

        public CandidateEmployeeConversionRepository(HRMSDbContext db)
        {
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> GetByCandidateApplicationId(int candidateApplicationId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryFirstOrDefaultAsync<CandidateEmployeeConversion>("GetCandidateEmployeeConversionByApplicationId", new { CandidateApplicationId = candidateApplicationId }, commandType: CommandType.StoredProcedure);
                response.isSuccess = result != null;
                response.ResponseMessage = result != null ? "Success!" : "No conversion recorded yet.";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> CreateConversion(CandidateEmployeeConversion model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "INSERT");
                parameters.Add("@CandidateApplicationId", model.CandidateApplicationId);
                parameters.Add("@EmployeeId", model.EmployeeId);
                parameters.Add("@EmployeeCode", model.EmployeeCode);
                parameters.Add("@AlfaEmployeeCode", model.AlfaEmployeeCode);
                parameters.Add("@ConversionStatus", model.ConversionStatus);
                parameters.Add("@FailureReason", model.FailureReason);
                parameters.Add("@CreatedBy", model.CreatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_CandidateEmployeeConversion_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> UpdateConversion(CandidateEmployeeConversion model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "UPDATE");
                parameters.Add("@CandidateEmployeeConversionId", model.CandidateEmployeeConversionId);
                parameters.Add("@EmployeeId", model.EmployeeId);
                parameters.Add("@EmployeeCode", model.EmployeeCode);
                parameters.Add("@AlfaEmployeeCode", model.AlfaEmployeeCode);
                parameters.Add("@ConversionStatus", model.ConversionStatus);
                parameters.Add("@FailureReason", model.FailureReason);
                parameters.Add("@UpdatedBy", model.UpdatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_CandidateEmployeeConversion_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }
    }
}
