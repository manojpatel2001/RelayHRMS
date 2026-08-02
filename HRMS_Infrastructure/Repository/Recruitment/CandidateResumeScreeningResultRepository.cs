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
    public class CandidateResumeScreeningResultRepository : ICandidateResumeScreeningResultRepository
    {
        private readonly string _connectionString;

        public CandidateResumeScreeningResultRepository(HRMSDbContext db)
        {
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> CreateOrUpdateScreeningResult(CandidateResumeScreeningResult model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "INSERT");
                parameters.Add("@CandidateApplicationId", model.CandidateApplicationId);
                parameters.Add("@JobPositionId", model.JobPositionId);
                parameters.Add("@SkillMatchScore", model.SkillMatchScore);
                parameters.Add("@ExperienceMatchScore", model.ExperienceMatchScore);
                parameters.Add("@EducationMatchScore", model.EducationMatchScore);
                parameters.Add("@OverallScore", model.OverallScore);
                parameters.Add("@Recommendation", model.Recommendation);
                parameters.Add("@MatchedSkills", model.MatchedSkills);
                parameters.Add("@MissingSkills", model.MissingSkills);
                parameters.Add("@IsDuplicate", model.IsDuplicate);
                parameters.Add("@DuplicateOfCandidateId", model.DuplicateOfCandidateId);
                parameters.Add("@DuplicateMatchReason", model.DuplicateMatchReason);
                parameters.Add("@ScreeningEngineVersion", model.ScreeningEngineVersion);
                parameters.Add("@CreatedBy", model.CreatedBy);
                parameters.Add("@UpdatedBy", model.UpdatedBy ?? model.CreatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_CandidateResumeScreeningResult_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> GetScreeningResultByApplicationId(int candidateApplicationId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryFirstOrDefaultAsync<CandidateResumeScreeningResult>("GetCandidateResumeScreeningResultByApplicationId",
                    new { CandidateApplicationId = candidateApplicationId }, commandType: CommandType.StoredProcedure);
                response.isSuccess = result != null;
                response.ResponseMessage = result != null ? "Success!" : "No screening result found.";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> GetScreeningResultsByJobPositionId(int jobPositionId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync("GetScreeningResultsByJobPositionId", new { JobPositionId = jobPositionId }, commandType: CommandType.StoredProcedure);
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
