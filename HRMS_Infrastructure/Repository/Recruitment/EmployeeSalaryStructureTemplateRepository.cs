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
    public class EmployeeSalaryStructureTemplateRepository : IEmployeeSalaryStructureTemplateRepository
    {
        private readonly string _connectionString;

        public EmployeeSalaryStructureTemplateRepository(HRMSDbContext db)
        {
            _connectionString = db.Database.GetDbConnection().ConnectionString;
        }

        public async Task<APIResponse> AssignTemplateToEmployee(EmployeeSalaryStructureTemplate model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "ASSIGN");
                parameters.Add("@EmployeeId", model.EmployeeId);
                parameters.Add("@SalaryStructureTemplateId", model.SalaryStructureTemplateId);
                parameters.Add("@CompanyId", model.CompanyId);
                parameters.Add("@CreatedBy", model.CreatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_EmployeeSalaryStructureTemplate_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> UnassignEmployeeTemplate(int employeeId, int? updatedBy)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "UNASSIGN");
                parameters.Add("@EmployeeId", employeeId);
                parameters.Add("@UpdatedBy", updatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_EmployeeSalaryStructureTemplate_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> GetEmployeeTemplate(int employeeId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryFirstOrDefaultAsync<EmployeeSalaryStructureTemplate>(
                    "GetEmployeeSalaryStructureTemplateByEmployeeId", new { EmployeeId = employeeId }, commandType: CommandType.StoredProcedure);
                response.isSuccess = result != null;
                response.ResponseMessage = result != null ? "Success!" : "No explicit template assigned to this employee.";
                response.Data = result;
            }
            catch (Exception ex)
            {
                response.isSuccess = false;
                response.ResponseMessage = ex.Message;
            }
            return response;
        }

        public async Task<EffectiveSalaryStructureTemplateResult?> GetEffectiveTemplate(int employeeId, int companyId, int? gradeId, int? designationId)
        {
            using var connection = new SqlConnection(_connectionString);
            var parameters = new DynamicParameters();
            parameters.Add("@EmployeeId", employeeId);
            parameters.Add("@CompanyId", companyId);
            parameters.Add("@GradeId", gradeId);
            parameters.Add("@DesignationId", designationId);

            var result = await connection.QueryFirstOrDefaultAsync<EffectiveSalaryStructureTemplateResult>(
                "GetEffectiveSalaryStructureTemplate", parameters, commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
