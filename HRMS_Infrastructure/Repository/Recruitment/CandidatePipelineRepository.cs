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
    public class CandidatePipelineRepository : ICandidatePipelineRepository
    {
        private readonly string _connectionString;

        public CandidatePipelineRepository(HRMSDbContext db)
        {
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> GetAllPipelineStages(int? companyId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<CandidatePipelineStage>("GetAllCandidatePipelineStages", new { CompanyId = companyId }, commandType: CommandType.StoredProcedure);
                response.isSuccess = true;
                response.ResponseMessage = "Success!";
                response.Data = result.AsList();
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> CreatePipelineStage(CandidatePipelineStage model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "INSERT");
                parameters.Add("@StageName", model.StageName);
                parameters.Add("@StageCategory", model.StageCategory);
                parameters.Add("@SortOrder", model.SortOrder);
                parameters.Add("@IsTerminal", model.IsTerminal);
                parameters.Add("@CompanyId", model.CompanyId);
                parameters.Add("@IsEnabled", model.IsEnabled);
                parameters.Add("@CreatedBy", model.CreatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_CandidatePipelineStage_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> UpdatePipelineStage(CandidatePipelineStage model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "UPDATE");
                parameters.Add("@CandidatePipelineStageId", model.CandidatePipelineStageId);
                parameters.Add("@StageName", model.StageName);
                parameters.Add("@StageCategory", model.StageCategory);
                parameters.Add("@SortOrder", model.SortOrder);
                parameters.Add("@IsTerminal", model.IsTerminal);
                parameters.Add("@IsEnabled", model.IsEnabled);
                parameters.Add("@UpdatedBy", model.UpdatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_CandidatePipelineStage_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> MoveCandidateToStage(int candidateApplicationId, int toStageId, string? remarks, int? actionBy)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@CandidateApplicationId", candidateApplicationId);
                parameters.Add("@ToStageId", toStageId);
                parameters.Add("@Remarks", remarks);
                parameters.Add("@ActionBy", actionBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_MoveCandidatePipelineStage", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> GetPipelineHistoryByApplicationId(int candidateApplicationId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<CandidatePipelineHistory>("GetCandidatePipelineHistoryByApplicationId",
                    new { CandidateApplicationId = candidateApplicationId }, commandType: CommandType.StoredProcedure);
                response.isSuccess = true;
                response.ResponseMessage = "Success!";
                response.Data = result.AsList();
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }
    }
}
