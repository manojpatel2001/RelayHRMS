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
    public class JobPositionRepository : IJobPositionRepository
    {
        private readonly string _connectionString;

        public JobPositionRepository(HRMSDbContext db)
        {
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> GetAllJobPositions(CommonParameter commonParameter)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@CompanyId", commonParameter?.CompanyId);
                parameters.Add("@BranchId", commonParameter?.BranchId);
                parameters.Add("@Status", (string?)null);

                var result = await connection.QueryAsync<JobPositionMaster>("GetAllJobPositionMaster", parameters, commandType: CommandType.StoredProcedure);
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

        public async Task<APIResponse> GetJobPositionById(int jobPositionId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryFirstOrDefaultAsync<JobPositionMaster>("GetJobPositionMasterById",
                    new { JobPositionId = jobPositionId }, commandType: CommandType.StoredProcedure);
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

        public async Task<APIResponse> CreateJobPosition(JobPositionMaster model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "INSERT");
                AddCommonParams(parameters, model);
                parameters.Add("@CreatedBy", model.CreatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_JobPositionMaster_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> UpdateJobPosition(JobPositionMaster model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "UPDATE");
                parameters.Add("@JobPositionId", model.JobPositionId);
                AddCommonParams(parameters, model);
                parameters.Add("@UpdatedBy", model.UpdatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_JobPositionMaster_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> DeleteJobPosition(DeleteRecordVM model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "DELETE");
                parameters.Add("@JobPositionId", model.Id);
                parameters.Add("@DeletedBy", string.IsNullOrEmpty(model.DeletedBy) ? (int?)null : int.Parse(model.DeletedBy));

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_JobPositionMaster_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> GetJobPositionDropdown(int companyId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync("GetJobPositionDropdown", new { CompanyId = companyId }, commandType: CommandType.StoredProcedure);
                response.isSuccess = true;
                response.ResponseMessage = "Success!";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<APIResponse> GetJobPositionsByManpowerRequisitionId(int manpowerRequisitionId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<JobPositionMaster>("GetJobPositionsByManpowerRequisitionId",
                    new { ManpowerRequisitionId = manpowerRequisitionId }, commandType: CommandType.StoredProcedure);
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

        private static void AddCommonParams(DynamicParameters parameters, JobPositionMaster model)
        {
            parameters.Add("@ManpowerRequisitionId", model.ManpowerRequisitionId);
            parameters.Add("@PositionTitle", model.PositionTitle);
            parameters.Add("@DesignationId", model.DesignationId);
            parameters.Add("@DepartmentId", model.DepartmentId);
            parameters.Add("@BranchId", model.BranchId);
            parameters.Add("@CompanyId", model.CompanyId);
            parameters.Add("@GradeId", model.GradeId);
            parameters.Add("@EmploymentType", model.EmploymentType);
            parameters.Add("@VacancyCount", model.VacancyCount);
            parameters.Add("@MinExperienceYears", model.MinExperienceYears);
            parameters.Add("@MaxExperienceYears", model.MaxExperienceYears);
            parameters.Add("@MinEducationLevel", model.MinEducationLevel);
            parameters.Add("@RequiredSkills", model.RequiredSkills);
            parameters.Add("@PreferredSkills", model.PreferredSkills);
            parameters.Add("@JobDescription", model.JobDescription);
            parameters.Add("@MinBudget", model.MinBudget);
            parameters.Add("@MaxBudget", model.MaxBudget);
            parameters.Add("@Priority", model.Priority);
            parameters.Add("@OwnerRecruiterId", model.OwnerRecruiterId);
            parameters.Add("@ReportingManagerId", model.ReportingManagerId);
            parameters.Add("@Status", model.Status);
            parameters.Add("@TargetClosureDate", model.TargetClosureDate);
            parameters.Add("@IsEnabled", model.IsEnabled);
        }
    }
}
