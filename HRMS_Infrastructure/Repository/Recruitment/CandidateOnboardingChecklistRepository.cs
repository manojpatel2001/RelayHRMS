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
    public class CandidateOnboardingChecklistRepository : ICandidateOnboardingChecklistRepository
    {
        private readonly string _connectionString;

        public CandidateOnboardingChecklistRepository(HRMSDbContext db)
        {
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> GetByCandidateApplicationId(int candidateApplicationId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryFirstOrDefaultAsync<CandidateOnboardingChecklist>("GetCandidateOnboardingChecklistByApplicationId", new { CandidateApplicationId = candidateApplicationId }, commandType: CommandType.StoredProcedure);
                response.isSuccess = result != null;
                response.ResponseMessage = result != null ? "Success!" : "No checklist created yet.";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> CreateOrUpdateChecklist(CandidateOnboardingChecklist model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "INSERT");
                parameters.Add("@CandidateOnboardingChecklistId", model.CandidateOnboardingChecklistId);
                parameters.Add("@CandidateApplicationId", model.CandidateApplicationId);
                parameters.Add("@OfferAcceptedConfirmed", model.OfferAcceptedConfirmed);
                parameters.Add("@DocumentsComplete", model.DocumentsComplete);
                parameters.Add("@BGVComplete", model.BGVComplete);
                parameters.Add("@MedicalComplete", model.MedicalComplete);
                parameters.Add("@SalaryApproved", model.SalaryApproved);
                parameters.Add("@JoiningApproved", model.JoiningApproved);
                parameters.Add("@EmployeeCreationApproved", model.EmployeeCreationApproved);
                parameters.Add("@FinalRemarks", model.FinalRemarks);
                parameters.Add("@FinalApprovedBy", model.FinalApprovedBy);
                parameters.Add("@JoiningStatus", model.JoiningStatus);
                parameters.Add("@ActualJoiningDate", model.ActualJoiningDate);
                parameters.Add("@IsEnabled", model.IsEnabled);
                parameters.Add("@CreatedBy", model.CreatedBy);
                parameters.Add("@UpdatedBy", model.UpdatedBy ?? model.CreatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_CandidateOnboardingChecklist_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }
    }
}
