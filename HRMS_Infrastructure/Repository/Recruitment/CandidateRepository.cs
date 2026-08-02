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
    public class CandidateRepository : ICandidateRepository
    {
        private readonly string _connectionString;

        public CandidateRepository(HRMSDbContext db)
        {
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> GetAllCandidates()
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<Candidate>("GetAllCandidates", commandType: CommandType.StoredProcedure);
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

        public async Task<APIResponse> GetCandidateById(int candidateId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryFirstOrDefaultAsync<Candidate>("GetCandidateById", new { CandidateId = candidateId }, commandType: CommandType.StoredProcedure);
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

        public async Task<APIResponse> GetCandidateFullProfile(int candidateId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var candidate = await connection.QueryFirstOrDefaultAsync<Candidate>("GetCandidateById", new { CandidateId = candidateId }, commandType: CommandType.StoredProcedure);
                if (candidate == null)
                {
                    response.isSuccess = false;
                    response.ResponseMessage = "No record found.";
                    return response;
                }

                var education = await connection.QueryAsync<CandidateEducation>("GetCandidateEducationByCandidateId", new { CandidateId = candidateId }, commandType: CommandType.StoredProcedure);
                var experience = await connection.QueryAsync<CandidateExperience>("GetCandidateExperienceByCandidateId", new { CandidateId = candidateId }, commandType: CommandType.StoredProcedure);
                var skills = await connection.QueryAsync<CandidateSkill>("GetCandidateSkillByCandidateId", new { CandidateId = candidateId }, commandType: CommandType.StoredProcedure);
                var certifications = await connection.QueryAsync<CandidateCertification>("GetCandidateCertificationByCandidateId", new { CandidateId = candidateId }, commandType: CommandType.StoredProcedure);
                var projects = await connection.QueryAsync<CandidateProject>("GetCandidateProjectByCandidateId", new { CandidateId = candidateId }, commandType: CommandType.StoredProcedure);

                response.isSuccess = true;
                response.ResponseMessage = "Success!";
                response.Data = new
                {
                    Candidate = candidate,
                    Education = education.AsList(),
                    Experience = experience.AsList(),
                    Skills = skills.AsList(),
                    Certifications = certifications.AsList(),
                    Projects = projects.AsList()
                };
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> CreateCandidate(Candidate model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "INSERT");
                AddCommonParams(parameters, model);
                parameters.Add("@CreatedBy", model.CreatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_Candidate_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> UpdateCandidate(Candidate model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "UPDATE");
                parameters.Add("@CandidateId", model.CandidateId);
                AddCommonParams(parameters, model);
                parameters.Add("@UpdatedBy", model.UpdatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_Candidate_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> DeleteCandidate(DeleteRecordVM model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "DELETE");
                parameters.Add("@CandidateId", model.Id);
                parameters.Add("@DeletedBy", string.IsNullOrEmpty(model.DeletedBy) ? (int?)null : int.Parse(model.DeletedBy));

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_Candidate_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> CheckDuplicateCandidate(string? email, string? phone, string? resumeFileHash)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync("CheckCandidateDuplicate",
                    new { Email = email, Phone = phone, ResumeFileHash = resumeFileHash }, commandType: CommandType.StoredProcedure);
                var list = result.AsList();
                response.isSuccess = true;
                response.ResponseMessage = list.Count > 0 ? "Potential duplicate candidate(s) found." : "No duplicates found.";
                response.Data = list;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        private static void AddCommonParams(DynamicParameters parameters, Candidate model)
        {
            parameters.Add("@FirstName", model.FirstName);
            parameters.Add("@MiddleName", model.MiddleName);
            parameters.Add("@LastName", model.LastName);
            parameters.Add("@Email", model.Email);
            parameters.Add("@Phone", model.Phone);
            parameters.Add("@AlternatePhone", model.AlternatePhone);
            parameters.Add("@DateOfBirth", model.DateOfBirth);
            parameters.Add("@Gender", model.Gender);
            parameters.Add("@CurrentAddress", model.CurrentAddress);
            parameters.Add("@CurrentCity", model.CurrentCity);
            parameters.Add("@PreferredLocation", model.PreferredLocation);
            parameters.Add("@CurrentEmployer", model.CurrentEmployer);
            parameters.Add("@CurrentDesignation", model.CurrentDesignation);
            parameters.Add("@TotalExperienceYears", model.TotalExperienceYears);
            parameters.Add("@RelevantExperienceYears", model.RelevantExperienceYears);
            parameters.Add("@CurrentCTC", model.CurrentCTC);
            parameters.Add("@ExpectedCTC", model.ExpectedCTC);
            parameters.Add("@NoticePeriodDays", model.NoticePeriodDays);
            parameters.Add("@LinkedInUrl", model.LinkedInUrl);
            parameters.Add("@GitHubUrl", model.GitHubUrl);
            parameters.Add("@PortfolioUrl", model.PortfolioUrl);
            parameters.Add("@ResumeUrl", model.ResumeUrl);
            parameters.Add("@ResumeFileHash", model.ResumeFileHash);
            parameters.Add("@Source", model.Source);
            parameters.Add("@ReferredByEmployeeId", model.ReferredByEmployeeId);
            parameters.Add("@Summary", model.Summary);
            parameters.Add("@IsBlacklisted", model.IsBlacklisted);
            parameters.Add("@IsEnabled", model.IsEnabled);
        }
    }
}
