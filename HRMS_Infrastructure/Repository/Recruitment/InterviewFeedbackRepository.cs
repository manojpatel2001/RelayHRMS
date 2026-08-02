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
    public class InterviewFeedbackRepository : IInterviewFeedbackRepository
    {
        private readonly string _connectionString;

        public InterviewFeedbackRepository(HRMSDbContext db)
        {
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> SubmitFeedback(InterviewFeedback model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "INSERT");
                parameters.Add("@InterviewId", model.InterviewId);
                parameters.Add("@PanelistEmployeeId", model.PanelistEmployeeId);
                parameters.Add("@TechnicalRating", model.TechnicalRating);
                parameters.Add("@CommunicationRating", model.CommunicationRating);
                parameters.Add("@ProblemSolvingRating", model.ProblemSolvingRating);
                parameters.Add("@OverallRating", model.OverallRating);
                parameters.Add("@Strengths", model.Strengths);
                parameters.Add("@Weaknesses", model.Weaknesses);
                parameters.Add("@Recommendation", model.Recommendation);
                parameters.Add("@Remarks", model.Remarks);
                parameters.Add("@IsSubmitted", model.IsSubmitted);
                parameters.Add("@CreatedBy", model.CreatedBy);
                parameters.Add("@UpdatedBy", model.UpdatedBy ?? model.CreatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_InterviewFeedback_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> GetFeedbackByInterviewId(int interviewId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<InterviewFeedback>("GetInterviewFeedbackByInterviewId", new { InterviewId = interviewId }, commandType: CommandType.StoredProcedure);
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

        public async Task<APIResponse> GetConsolidatedFeedbackByCandidateApplicationId(int candidateApplicationId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<InterviewFeedback>("GetConsolidatedFeedbackByCandidateApplicationId", new { CandidateApplicationId = candidateApplicationId }, commandType: CommandType.StoredProcedure);
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
