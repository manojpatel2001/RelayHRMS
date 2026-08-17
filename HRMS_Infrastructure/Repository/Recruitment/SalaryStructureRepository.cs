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
    public class SalaryStructureRepository : ISalaryStructureRepository
    {
        private readonly string _connectionString;
        private readonly ICompanyStatutorySettingRepository _statutorySettingRepository;
        private readonly SalaryStructureCalculationService _calculationService;

        public SalaryStructureRepository(HRMSDbContext db)
        {
            _connectionString = db.Database.GetDbConnection().ConnectionString;
            _statutorySettingRepository = new CompanyStatutorySettingRepository(db);
            _calculationService = new SalaryStructureCalculationService();
        }

        public async Task<APIResponse> GetAllTemplates(int? companyId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<SalaryStructureTemplate>("GetAllSalaryStructureTemplates", new { CompanyId = companyId }, commandType: CommandType.StoredProcedure);
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

        public async Task<APIResponse> GetTemplateById(int salaryStructureTemplateId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryFirstOrDefaultAsync<SalaryStructureTemplate>("GetSalaryStructureTemplateById", new { SalaryStructureTemplateId = salaryStructureTemplateId }, commandType: CommandType.StoredProcedure);
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

        public async Task<APIResponse> CreateTemplate(SalaryStructureTemplate model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "INSERT");
                parameters.Add("@TemplateName", model.TemplateName);
                parameters.Add("@GradeId", model.GradeId);
                parameters.Add("@DesignationId", model.DesignationId);
                parameters.Add("@CompanyId", model.CompanyId);
                parameters.Add("@IsDefault", model.IsDefault);
                parameters.Add("@Status", model.Status);
                parameters.Add("@IsEnabled", model.IsEnabled);
                parameters.Add("@CreatedBy", model.CreatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_SalaryStructureTemplate_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> UpdateTemplate(SalaryStructureTemplate model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "UPDATE");
                parameters.Add("@SalaryStructureTemplateId", model.SalaryStructureTemplateId);
                parameters.Add("@TemplateName", model.TemplateName);
                parameters.Add("@GradeId", model.GradeId);
                parameters.Add("@DesignationId", model.DesignationId);
                parameters.Add("@IsDefault", model.IsDefault);
                parameters.Add("@Status", model.Status);
                parameters.Add("@IsEnabled", model.IsEnabled);
                parameters.Add("@UpdatedBy", model.UpdatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_SalaryStructureTemplate_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> DeleteTemplate(DeleteRecordVM model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "DELETE");
                parameters.Add("@SalaryStructureTemplateId", model.Id);
                parameters.Add("@DeletedBy", string.IsNullOrEmpty(model.DeletedBy) ? (int?)null : int.Parse(model.DeletedBy));

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_SalaryStructureTemplate_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> CreateComponent(SalaryStructureComponent model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "INSERT");
                parameters.Add("@SalaryStructureTemplateId", model.SalaryStructureTemplateId);
                parameters.Add("@ComponentName", model.ComponentName);
                parameters.Add("@ComponentCategory", model.ComponentCategory);
                parameters.Add("@CalculationType", model.CalculationType);
                parameters.Add("@Value", model.Value);
                parameters.Add("@SortOrder", model.SortOrder);
                parameters.Add("@IsEnabled", model.IsEnabled);
                parameters.Add("@CreatedBy", model.CreatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_SalaryStructureComponent_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> UpdateComponent(SalaryStructureComponent model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "UPDATE");
                parameters.Add("@SalaryStructureComponentId", model.SalaryStructureComponentId);
                parameters.Add("@ComponentName", model.ComponentName);
                parameters.Add("@ComponentCategory", model.ComponentCategory);
                parameters.Add("@CalculationType", model.CalculationType);
                parameters.Add("@Value", model.Value);
                parameters.Add("@SortOrder", model.SortOrder);
                parameters.Add("@IsEnabled", model.IsEnabled);
                parameters.Add("@UpdatedBy", model.UpdatedBy);

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_SalaryStructureComponent_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> DeleteComponent(DeleteRecordVM model)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var parameters = new DynamicParameters();
                parameters.Add("@Operation", "DELETE");
                parameters.Add("@SalaryStructureComponentId", model.Id);
                parameters.Add("@DeletedBy", string.IsNullOrEmpty(model.DeletedBy) ? (int?)null : int.Parse(model.DeletedBy));

                var row = await connection.QueryFirstOrDefaultAsync<dynamic>("sp_SalaryStructureComponent_CRUD", parameters, commandType: CommandType.StoredProcedure);
                return RecruitmentSqlHelper.ToApiResponse(row);
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = ex.Message };
            }
        }

        public async Task<APIResponse> GetComponentsByTemplateId(int salaryStructureTemplateId)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);
                var result = await connection.QueryAsync<SalaryStructureComponent>("GetSalaryStructureComponentsByTemplateId", new { SalaryStructureTemplateId = salaryStructureTemplateId }, commandType: CommandType.StoredProcedure);
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

        // Computes the full CTC breakdown for an EXPLICITLY chosen template —
        // no employee/grade resolution involved — so HR can review a template
        // on its own before assigning it to anyone. Reuses the exact same
        // SalaryStructureCalculationService as Employee Master's live
        // allowance preview (see EmployeeSalaryAllowanceRepository.ComputeAsync),
        // just addressed by TemplateId instead of a resolved one.
        public async Task<APIResponse> PreviewAllowance(int salaryStructureTemplateId, decimal grossSalary, decimal? basicSalary, int companyId, bool isPFApplicable)
        {
            var response = new APIResponse();
            try
            {
                using var connection = new SqlConnection(_connectionString);

                var components = (await connection.QueryAsync<SalaryStructureComponent>(
                    "GetSalaryStructureComponentsByTemplateId", new { SalaryStructureTemplateId = salaryStructureTemplateId }, commandType: CommandType.StoredProcedure)).AsList();

                if (components.Count == 0)
                {
                    response.isSuccess = false;
                    response.ResponseMessage = "This template has no components configured yet — add some on the Salary Structure Master page first.";
                    return response;
                }

                var allowance = _calculationService.ComputeAllowance(components, grossSalary, manualBasicOverride: basicSalary);

                var statutorySetting = await _statutorySettingRepository.GetByCompanyId(companyId) ?? new CompanyStatutorySetting { CompanyId = companyId };

                var termInsurance = await connection.ExecuteScalarAsync<decimal?>(
                    "SELECT dbo.fn_GetTermInsuranceDeduction(@GrossSalary, @ProcessingDate, @CompanyId)",
                    new { GrossSalary = grossSalary, ProcessingDate = DateTime.Today, CompanyId = companyId }) ?? 0m;

                var deductions = _calculationService.ComputeStatutoryDeductions(statutorySetting, allowance.BasicSalary, allowance.TotalGrossSalary, grossSalary, isPFApplicable, termInsurance);

                response.isSuccess = true;
                response.ResponseMessage = "Success!";
                response.Data = new CTCCalculationPreviewResult
                {
                    BasicSalary = allowance.BasicSalary,
                    HRA = allowance.HRA,
                    ConveyanceAllowance = allowance.ConveyanceAllowance,
                    ChildEducationAllowance = allowance.ChildEducationAllowance,
                    MedicalAllowance = allowance.MedicalAllowance,
                    DeputationAllowance = allowance.DeputationAllowance,
                    TotalGrossSalary = allowance.TotalGrossSalary,
                    EmployeePF = deductions.EmployeePF,
                    EmployerPF = deductions.EmployerPF,
                    EmployeeESI = deductions.EmployeeESI,
                    EmployerESI = deductions.EmployerESI,
                    ProfessionalTax = deductions.ProfessionalTax,
                    GroupMedical = deductions.GroupMedical,
                    TermInsurance = deductions.TermInsurance,
                    TotalDeductions = deductions.TotalDeductions,
                    NetSalary = allowance.TotalGrossSalary - deductions.TotalDeductions,
                    CTC = allowance.TotalGrossSalary + deductions.EmployerPF + deductions.EmployerESI,
                    Components = allowance.Components
                };
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
