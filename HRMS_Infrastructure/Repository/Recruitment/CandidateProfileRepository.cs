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
    // Combined repository for the 5 identical-shape Candidate profile child tables.
    public class CandidateProfileRepository : ICandidateProfileRepository
    {
        private readonly string _connectionString;

        public CandidateProfileRepository(HRMSDbContext db)
        {
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        private static int? ParseDeletedBy(DeleteRecordVM model) =>
            string.IsNullOrEmpty(model.DeletedBy) ? (int?)null : int.Parse(model.DeletedBy);

        // ===== Education =====
        public async Task<APIResponse> CreateEducation(CandidateEducation model)
        {
            var p = new DynamicParameters();
            p.Add("@Operation", "INSERT");
            p.Add("@CandidateId", model.CandidateId);
            p.Add("@Degree", model.Degree);
            p.Add("@Specialization", model.Specialization);
            p.Add("@Institution", model.Institution);
            p.Add("@University", model.University);
            p.Add("@YearOfPassing", model.YearOfPassing);
            p.Add("@PercentageOrCGPA", model.PercentageOrCGPA);
            p.Add("@EducationLevel", model.EducationLevel);
            p.Add("@IsEnabled", model.IsEnabled);
            p.Add("@CreatedBy", model.CreatedBy);
            return await Execute("sp_CandidateEducation_CRUD", p);
        }

        public async Task<APIResponse> UpdateEducation(CandidateEducation model)
        {
            var p = new DynamicParameters();
            p.Add("@Operation", "UPDATE");
            p.Add("@CandidateEducationId", model.CandidateEducationId);
            p.Add("@Degree", model.Degree);
            p.Add("@Specialization", model.Specialization);
            p.Add("@Institution", model.Institution);
            p.Add("@University", model.University);
            p.Add("@YearOfPassing", model.YearOfPassing);
            p.Add("@PercentageOrCGPA", model.PercentageOrCGPA);
            p.Add("@EducationLevel", model.EducationLevel);
            p.Add("@IsEnabled", model.IsEnabled);
            p.Add("@UpdatedBy", model.UpdatedBy);
            return await Execute("sp_CandidateEducation_CRUD", p);
        }

        public async Task<APIResponse> DeleteEducation(DeleteRecordVM model)
        {
            var p = new DynamicParameters();
            p.Add("@Operation", "DELETE");
            p.Add("@CandidateEducationId", model.Id);
            p.Add("@DeletedBy", ParseDeletedBy(model));
            return await Execute("sp_CandidateEducation_CRUD", p);
        }

        public async Task<APIResponse> GetEducationByCandidateId(int candidateId) =>
            await QueryList<CandidateEducation>("GetCandidateEducationByCandidateId", candidateId);

        // ===== Experience =====
        public async Task<APIResponse> CreateExperience(CandidateExperience model)
        {
            var p = new DynamicParameters();
            p.Add("@Operation", "INSERT");
            p.Add("@CandidateId", model.CandidateId);
            p.Add("@CompanyName", model.CompanyName);
            p.Add("@Designation", model.Designation);
            p.Add("@StartDate", model.StartDate);
            p.Add("@EndDate", model.EndDate);
            p.Add("@IsCurrentEmployer", model.IsCurrentEmployer);
            p.Add("@DurationMonths", model.DurationMonths);
            p.Add("@Responsibilities", model.Responsibilities);
            p.Add("@Location", model.Location);
            p.Add("@IsEnabled", model.IsEnabled);
            p.Add("@CreatedBy", model.CreatedBy);
            return await Execute("sp_CandidateExperience_CRUD", p);
        }

        public async Task<APIResponse> UpdateExperience(CandidateExperience model)
        {
            var p = new DynamicParameters();
            p.Add("@Operation", "UPDATE");
            p.Add("@CandidateExperienceId", model.CandidateExperienceId);
            p.Add("@CompanyName", model.CompanyName);
            p.Add("@Designation", model.Designation);
            p.Add("@StartDate", model.StartDate);
            p.Add("@EndDate", model.EndDate);
            p.Add("@IsCurrentEmployer", model.IsCurrentEmployer);
            p.Add("@DurationMonths", model.DurationMonths);
            p.Add("@Responsibilities", model.Responsibilities);
            p.Add("@Location", model.Location);
            p.Add("@IsEnabled", model.IsEnabled);
            p.Add("@UpdatedBy", model.UpdatedBy);
            return await Execute("sp_CandidateExperience_CRUD", p);
        }

        public async Task<APIResponse> DeleteExperience(DeleteRecordVM model)
        {
            var p = new DynamicParameters();
            p.Add("@Operation", "DELETE");
            p.Add("@CandidateExperienceId", model.Id);
            p.Add("@DeletedBy", ParseDeletedBy(model));
            return await Execute("sp_CandidateExperience_CRUD", p);
        }

        public async Task<APIResponse> GetExperienceByCandidateId(int candidateId) =>
            await QueryList<CandidateExperience>("GetCandidateExperienceByCandidateId", candidateId);

        // ===== Skill =====
        public async Task<APIResponse> CreateSkill(CandidateSkill model)
        {
            var p = new DynamicParameters();
            p.Add("@Operation", "INSERT");
            p.Add("@CandidateId", model.CandidateId);
            p.Add("@SkillName", model.SkillName);
            p.Add("@ProficiencyLevel", model.ProficiencyLevel);
            p.Add("@ExperienceYears", model.ExperienceYears);
            p.Add("@IsEnabled", model.IsEnabled);
            p.Add("@CreatedBy", model.CreatedBy);
            return await Execute("sp_CandidateSkill_CRUD", p);
        }

        public async Task<APIResponse> UpdateSkill(CandidateSkill model)
        {
            var p = new DynamicParameters();
            p.Add("@Operation", "UPDATE");
            p.Add("@CandidateSkillId", model.CandidateSkillId);
            p.Add("@SkillName", model.SkillName);
            p.Add("@ProficiencyLevel", model.ProficiencyLevel);
            p.Add("@ExperienceYears", model.ExperienceYears);
            p.Add("@IsEnabled", model.IsEnabled);
            p.Add("@UpdatedBy", model.UpdatedBy);
            return await Execute("sp_CandidateSkill_CRUD", p);
        }

        public async Task<APIResponse> DeleteSkill(DeleteRecordVM model)
        {
            var p = new DynamicParameters();
            p.Add("@Operation", "DELETE");
            p.Add("@CandidateSkillId", model.Id);
            p.Add("@DeletedBy", ParseDeletedBy(model));
            return await Execute("sp_CandidateSkill_CRUD", p);
        }

        public async Task<APIResponse> GetSkillByCandidateId(int candidateId) =>
            await QueryList<CandidateSkill>("GetCandidateSkillByCandidateId", candidateId);

        // ===== Certification =====
        public async Task<APIResponse> CreateCertification(CandidateCertification model)
        {
            var p = new DynamicParameters();
            p.Add("@Operation", "INSERT");
            p.Add("@CandidateId", model.CandidateId);
            p.Add("@CertificationName", model.CertificationName);
            p.Add("@IssuingBody", model.IssuingBody);
            p.Add("@IssueDate", model.IssueDate);
            p.Add("@ExpiryDate", model.ExpiryDate);
            p.Add("@CertificateUrl", model.CertificateUrl);
            p.Add("@IsEnabled", model.IsEnabled);
            p.Add("@CreatedBy", model.CreatedBy);
            return await Execute("sp_CandidateCertification_CRUD", p);
        }

        public async Task<APIResponse> UpdateCertification(CandidateCertification model)
        {
            var p = new DynamicParameters();
            p.Add("@Operation", "UPDATE");
            p.Add("@CandidateCertificationId", model.CandidateCertificationId);
            p.Add("@CertificationName", model.CertificationName);
            p.Add("@IssuingBody", model.IssuingBody);
            p.Add("@IssueDate", model.IssueDate);
            p.Add("@ExpiryDate", model.ExpiryDate);
            p.Add("@CertificateUrl", model.CertificateUrl);
            p.Add("@IsEnabled", model.IsEnabled);
            p.Add("@UpdatedBy", model.UpdatedBy);
            return await Execute("sp_CandidateCertification_CRUD", p);
        }

        public async Task<APIResponse> DeleteCertification(DeleteRecordVM model)
        {
            var p = new DynamicParameters();
            p.Add("@Operation", "DELETE");
            p.Add("@CandidateCertificationId", model.Id);
            p.Add("@DeletedBy", ParseDeletedBy(model));
            return await Execute("sp_CandidateCertification_CRUD", p);
        }

        public async Task<APIResponse> GetCertificationByCandidateId(int candidateId) =>
            await QueryList<CandidateCertification>("GetCandidateCertificationByCandidateId", candidateId);

        // ===== Project =====
        public async Task<APIResponse> CreateProject(CandidateProject model)
        {
            var p = new DynamicParameters();
            p.Add("@Operation", "INSERT");
            p.Add("@CandidateId", model.CandidateId);
            p.Add("@ProjectName", model.ProjectName);
            p.Add("@Description", model.Description);
            p.Add("@RoleInProject", model.RoleInProject);
            p.Add("@TechnologiesUsed", model.TechnologiesUsed);
            p.Add("@StartDate", model.StartDate);
            p.Add("@EndDate", model.EndDate);
            p.Add("@ProjectUrl", model.ProjectUrl);
            p.Add("@IsEnabled", model.IsEnabled);
            p.Add("@CreatedBy", model.CreatedBy);
            return await Execute("sp_CandidateProject_CRUD", p);
        }

        public async Task<APIResponse> UpdateProject(CandidateProject model)
        {
            var p = new DynamicParameters();
            p.Add("@Operation", "UPDATE");
            p.Add("@CandidateProjectId", model.CandidateProjectId);
            p.Add("@ProjectName", model.ProjectName);
            p.Add("@Description", model.Description);
            p.Add("@RoleInProject", model.RoleInProject);
            p.Add("@TechnologiesUsed", model.TechnologiesUsed);
            p.Add("@StartDate", model.StartDate);
            p.Add("@EndDate", model.EndDate);
            p.Add("@ProjectUrl", model.ProjectUrl);
            p.Add("@IsEnabled", model.IsEnabled);
            p.Add("@UpdatedBy", model.UpdatedBy);
            return await Execute("sp_CandidateProject_CRUD", p);
        }

        public async Task<APIResponse> DeleteProject(DeleteRecordVM model)
        {
            var p = new DynamicParameters();
            p.Add("@Operation", "DELETE");
            p.Add("@CandidateProjectId", model.Id);
            p.Add("@DeletedBy", ParseDeletedBy(model));
            return await Execute("sp_CandidateProject_CRUD", p);
        }

        public async Task<APIResponse> GetProjectByCandidateId(int candidateId) =>
            await QueryList<CandidateProject>("GetCandidateProjectByCandidateId", candidateId);

        // ===== Shared helpers =====
        private async Task<APIResponse> Execute(string procName, DynamicParameters parameters)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var row = await connection.QueryFirstOrDefaultAsync<dynamic>(procName, parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        private async Task<APIResponse> QueryList<T>(string procName, int candidateId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<T>(procName, new { CandidateId = candidateId }, commandType: CommandType.StoredProcedure);
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
