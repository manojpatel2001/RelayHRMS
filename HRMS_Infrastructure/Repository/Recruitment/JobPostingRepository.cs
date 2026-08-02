using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface.Recruitment;
using HRMS_Utility;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.Recruitment
{
    public class JobPostingRepository : IJobPostingRepository
    {
        private readonly string _connectionString;

        public JobPostingRepository(HRMSDbContext db)
        {
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> GetAllJobPostings(CommonParameter commonParameter)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<JobPosting>("GetAllJobPosting", new { CompanyId = commonParameter?.CompanyId }, commandType: CommandType.StoredProcedure);
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

        public async Task<APIResponse> GetJobPostingById(int jobPostingId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryFirstOrDefaultAsync<JobPosting>("GetJobPostingById", new { JobPostingId = jobPostingId }, commandType: CommandType.StoredProcedure);
                response.isSuccess = result != null;
                response.ResponseMessage = result != null ? "Success!" : "No record found.";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> GetJobPostingBySlug(string slug)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryFirstOrDefaultAsync<JobPosting>("GetJobPostingBySlug", new { Slug = slug }, commandType: CommandType.StoredProcedure);
                response.isSuccess = result != null;
                response.ResponseMessage = result != null ? "Success!" : "No record found.";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> CreateJobPosting(JobPosting model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "INSERT");
                AddCommonParams(parameters, model);
                parameters.Add("@CreatedBy", model.CreatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_JobPosting_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> UpdateJobPosting(JobPosting model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "UPDATE");
                parameters.Add("@JobPostingId", model.JobPostingId);
                AddCommonParams(parameters, model);
                parameters.Add("@UpdatedBy", model.UpdatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_JobPosting_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> DeleteJobPosting(DeleteRecordVM model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "DELETE");
                parameters.Add("@JobPostingId", model.Id);
                parameters.Add("@DeletedBy", string.IsNullOrEmpty(model.DeletedBy) ? (int?)null : int.Parse(model.DeletedBy));

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_JobPosting_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> PublishJobPosting(int jobPostingId, int? updatedBy)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "PUBLISH");
                parameters.Add("@JobPostingId", jobPostingId);
                parameters.Add("@UpdatedBy", updatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_JobPosting_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        // Placeholder integration point — no live LinkedIn/Naukri/Indeed/Referral API call yet.
        // Records the channel as "posted" so the UI can reflect intent; a real integration
        // would replace the body of this method only.
        public async Task<APIResponse> PostExternally(int jobPostingId, string channel, string? postingUrl, int? updatedBy)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var existing = await connection.QueryFirstOrDefaultAsync<JobPosting>("GetJobPostingById", new { JobPostingId = jobPostingId }, commandType: CommandType.StoredProcedure);
                if (existing == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Job posting not found." };

                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "UPDATE");
                parameters.Add("@JobPostingId", jobPostingId);
                AddCommonParams(parameters, existing);
                parameters.Add("@UpdatedBy", updatedBy);

                switch (channel?.ToLowerInvariant())
                {
                    case "linkedin":
                        parameters.Add("@LinkedInEnabled", true); parameters.Add("@LinkedInStatus", "Posted");
                        parameters.Add("@LinkedInPostedDate", DateTime.UtcNow); parameters.Add("@LinkedInPostingUrl", postingUrl);
                        break;
                    case "naukri":
                        parameters.Add("@NaukriEnabled", true); parameters.Add("@NaukriStatus", "Posted");
                        parameters.Add("@NaukriPostedDate", DateTime.UtcNow); parameters.Add("@NaukriPostingUrl", postingUrl);
                        break;
                    case "indeed":
                        parameters.Add("@IndeedEnabled", true); parameters.Add("@IndeedStatus", "Posted");
                        parameters.Add("@IndeedPostedDate", DateTime.UtcNow); parameters.Add("@IndeedPostingUrl", postingUrl);
                        break;
                    case "referral":
                        parameters.Add("@ReferralEnabled", true); parameters.Add("@ReferralStatus", "Posted");
                        parameters.Add("@ReferralPostedDate", DateTime.UtcNow); parameters.Add("@ReferralPostingUrl", postingUrl);
                        break;
                    default:
                        return new APIResponse { isSuccess = false, ResponseMessage = "Unknown channel." };
                }

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_JobPosting_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        private static void AddCommonParams(DynamicParameters parameters, JobPosting model)
        {
            parameters.Add("@JobPositionId", model.JobPositionId);
            parameters.Add("@PostingTitle", model.PostingTitle);
            parameters.Add("@Slug", model.Slug);
            parameters.Add("@IsInternal", model.IsInternal);
            parameters.Add("@IsExternal", model.IsExternal);
            parameters.Add("@JobDescriptionHtml", model.JobDescriptionHtml);
            parameters.Add("@ResponsibilitiesHtml", model.ResponsibilitiesHtml);
            parameters.Add("@BenefitsHtml", model.BenefitsHtml);
            parameters.Add("@Location", model.Location);
            parameters.Add("@WorkMode", model.WorkMode);
            parameters.Add("@PublishDate", model.PublishDate);
            parameters.Add("@ExpiryDate", model.ExpiryDate);
            parameters.Add("@Status", model.Status);
            parameters.Add("@CompanyId", model.CompanyId);
            parameters.Add("@LinkedInEnabled", model.LinkedInEnabled);
            parameters.Add("@NaukriEnabled", model.NaukriEnabled);
            parameters.Add("@IndeedEnabled", model.IndeedEnabled);
            parameters.Add("@ReferralEnabled", model.ReferralEnabled);
            parameters.Add("@IsEnabled", model.IsEnabled);
        }
    }
}
