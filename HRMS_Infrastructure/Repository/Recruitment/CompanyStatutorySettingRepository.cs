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
    public class CompanyStatutorySettingRepository : ICompanyStatutorySettingRepository
    {
        private readonly string _connectionString;

        public CompanyStatutorySettingRepository(HRMSDbContext db)
        {
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> GetAll()
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<CompanyStatutorySetting>("GetAllCompanyStatutorySettings", commandType: CommandType.StoredProcedure);
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

        public async Task<CompanyStatutorySetting?> GetByCompanyId(int companyId)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<CompanyStatutorySetting>(
                "GetCompanyStatutorySettingByCompanyId", new { CompanyId = companyId }, commandType: CommandType.StoredProcedure);
        }

        public async Task<APIResponse> Create(CompanyStatutorySetting model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = BuildParameters(model, "INSERT");
                parameters.Add("@CreatedBy", model.CreatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_CompanyStatutorySetting_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> Update(CompanyStatutorySetting model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = BuildParameters(model, "UPDATE");
                parameters.Add("@CompanyStatutorySettingId", model.CompanyStatutorySettingId);
                parameters.Add("@UpdatedBy", model.UpdatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_CompanyStatutorySetting_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> Delete(DeleteRecordVM model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "DELETE");
                parameters.Add("@CompanyStatutorySettingId", model.Id);
                parameters.Add("@DeletedBy", string.IsNullOrEmpty(model.DeletedBy) ? (int?)null : int.Parse(model.DeletedBy));

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_CompanyStatutorySetting_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        private static DynamicParameters BuildParameters(CompanyStatutorySetting model, string operation)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Operation", operation);
            parameters.Add("@CompanyId", model.CompanyId);
            parameters.Add("@IsPFEnabled", model.IsPFEnabled);
            parameters.Add("@PFPercentage", model.PFPercentage);
            parameters.Add("@PFCapAmount", model.PFCapAmount);
            parameters.Add("@IsESIEnabled", model.IsESIEnabled);
            parameters.Add("@ESIPercentage", model.ESIPercentage);
            parameters.Add("@EmployerESIPercentage", model.EmployerESIPercentage);
            parameters.Add("@ESIGrossCeiling", model.ESIGrossCeiling);
            parameters.Add("@IsProfessionalTaxEnabled", model.IsProfessionalTaxEnabled);
            parameters.Add("@ProfessionalTaxThreshold", model.ProfessionalTaxThreshold);
            parameters.Add("@ProfessionalTaxAmount", model.ProfessionalTaxAmount);
            parameters.Add("@IsGroupMedicalEnabled", model.IsGroupMedicalEnabled);
            parameters.Add("@GroupMedicalGrossThreshold", model.GroupMedicalGrossThreshold);
            parameters.Add("@GroupMedicalAmount", model.GroupMedicalAmount);
            parameters.Add("@IsTermInsuranceEnabled", model.IsTermInsuranceEnabled);
            parameters.Add("@IsEnabled", model.IsEnabled);
            return parameters;
        }
    }
}
