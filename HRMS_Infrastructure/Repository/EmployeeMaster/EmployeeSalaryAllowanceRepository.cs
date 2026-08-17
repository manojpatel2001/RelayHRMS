using Dapper;
using HRMS_Core.DbContext;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Infrastructure.Interface.EmployeeMaster;
using HRMS_Infrastructure.Interface.Recruitment;
using HRMS_Infrastructure.Repository.Recruitment;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Repository.EmployeeMaster
{
    // Allowance is now driven by the employee's resolved SalaryStructureTemplate
    // (see IEmployeeSalaryStructureTemplateRepository.GetEffectiveTemplate —
    // an explicit employee mapping, falling back to the Grade/Designation
    // default) instead of the hardcoded formula that used to live entirely
    // inside USP_CalculateSalaryStructure (Basic = 40% of Gross, flat
    // Conveyance/ChildEducation/Medical, `IF @CompanyId = 11`). That stored
    // procedure is left untouched on the database (scripted for reference at
    // HRMS_Infrastructure\SQL\StoredProcedures\USP_CalculateSalaryStructure.sql)
    // but is no longer called from here — persistence into EmployeeSalaryAllowance
    // / EmployeeSalaryHistory is done directly, fed by the new calculation
    // engine, so a re-save under the new system produces different numbers
    // only when the employee's resolved template actually differs from the
    // old hardcoded formula. Existing rows are left untouched until an
    // employee is next saved (Gross/Basic edited) or a new template is assigned.
    public class EmployeeSalaryAllowanceRepository : IEmployeeSalaryAllowanceRepository
    {
        private readonly HRMSDbContext _db;
        private readonly string _connectionString;
        private readonly IEmployeeSalaryStructureTemplateRepository _templateMappingRepository;
        private readonly ISalaryStructureRepository _salaryStructureRepository;
        private readonly ICompanyStatutorySettingRepository _statutorySettingRepository;
        private readonly SalaryStructureCalculationService _calculationService;

        public EmployeeSalaryAllowanceRepository(HRMSDbContext db)
        {
            _db = db;
            _connectionString = db.Database.GetDbConnection().ConnectionString;
            _templateMappingRepository = new EmployeeSalaryStructureTemplateRepository(db);
            _salaryStructureRepository = new SalaryStructureRepository(db);
            _statutorySettingRepository = new CompanyStatutorySettingRepository(db);
            _calculationService = new SalaryStructureCalculationService();
        }

        public async Task<vmGetLiveEmployeeSalaryAllowance?> GetLiveEmployeeSalaryAllowance(salaryPara salaryPara)
        {
            // EmployeeId is 0 during employee CREATE (no row exists yet) — that's
            // fine, the resolver simply finds no employee-level mapping and falls
            // back to the Grade/Designation default using the GradeId/DesignationId
            // passed in from the form.
            if (salaryPara.CompanyId == null || salaryPara.GrossSalary == null)
                return null;

            var computed = await ComputeAsync(salaryPara.EmployeeId ?? 0, salaryPara.CompanyId.Value, salaryPara.GrossSalary.Value, salaryPara.IsPFApplicable ?? true, salaryPara.GradeId, salaryPara.DesignationId, salaryPara.BasicSalary);
            if (computed == null)
                return null;

            return new vmGetLiveEmployeeSalaryAllowance
            {
                BasicSalary = computed.Allowance.BasicSalary,
                ChildEducationAllowance = computed.Allowance.ChildEducationAllowance,
                ConveyanceAllowance = computed.Allowance.ConveyanceAllowance,
                HRA = computed.Allowance.HRA,
                MedicalAllowance = computed.Allowance.MedicalAllowance,
                DeputationAllowance = computed.Allowance.DeputationAllowance,
                TotalGrossSalary = computed.Allowance.TotalGrossSalary,
                EmployeePF = computed.Deductions.EmployeePF,
                EmployeeESI = computed.Deductions.EmployeeESI,
                ProfessionalTax = computed.Deductions.ProfessionalTax,
                GroupMedical = computed.Deductions.GroupMedical,
                TermInsurance = computed.Deductions.TermInsurance,
                TotalDeductions = computed.Deductions.TotalDeductions,
                NetSalary = computed.Allowance.TotalGrossSalary - computed.Deductions.TotalDeductions,
                EmployerPF = computed.Deductions.EmployerPF,
                EmployerESI = computed.Deductions.EmployerESI,
                CTC = computed.Allowance.TotalGrossSalary + computed.Deductions.EmployerPF + computed.Deductions.EmployerESI
            };
        }

        public async Task<VMCommonResult> CreateEmployeeSalaryAllowance(vmEmployeeSalary vmEmployeeSalary)
        {
            if (vmEmployeeSalary.EmployeeId == null || vmEmployeeSalary.CompanyId == null || vmEmployeeSalary.GrossSalary == null)
                return new VMCommonResult { Id = 0 };

            var computed = await ComputeAsync(vmEmployeeSalary.EmployeeId.Value, vmEmployeeSalary.CompanyId.Value, vmEmployeeSalary.GrossSalary.Value, vmEmployeeSalary.IsPFApplicable ?? true, basicSalary: vmEmployeeSalary.BasicSalary);
            if (computed == null)
                return new VMCommonResult { Id = 0 };

            using var connection = new SqlConnection(_connectionString);

            var exists = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(1) FROM EmployeeSalaryAllowance WHERE EmployeeId = @EmployeeId AND CompanyId = @CompanyId AND IsDeleted = 0 AND IsEnabled = 1",
                new { EmployeeId = vmEmployeeSalary.EmployeeId, CompanyId = vmEmployeeSalary.CompanyId });

            if (exists == 0)
            {
                await connection.ExecuteAsync(@"
                    INSERT INTO EmployeeSalaryAllowance (
                        EmployeeId, CompanyId,
                        BasicSalary, ChildEducationAllowance, ConveyanceAllowance, HRA,
                        MedicalAllowance, DeputationAllowance, TotalGrossSalary,
                        EmployeePF, EmployeeESI, ProfessionalTax, GroupMedical, TermInsurance,
                        TotalDeductions, NetSalary,
                        EmployerPF, EmployerESI, CTC, CreatedDate, IsEnabled, IsDeleted
                    )
                    VALUES (
                        @EmployeeId, @CompanyId,
                        @BasicSalary, @ChildEducationAllowance, @ConveyanceAllowance, @HRA,
                        @MedicalAllowance, @DeputationAllowance, @TotalGrossSalary,
                        @EmployeePF, @EmployeeESI, @ProfessionalTax, @GroupMedical, @TermInsurance,
                        @TotalDeductions, @NetSalary,
                        @EmployerPF, @EmployerESI, @CTC, GETUTCDATE(), 1, 0
                    )", BuildPersistParameters(vmEmployeeSalary.EmployeeId.Value, vmEmployeeSalary.CompanyId.Value, computed));
            }

            return new VMCommonResult { Id = vmEmployeeSalary.EmployeeId };
        }

        public async Task<VMCommonResult> UpdateEmployeeSalaryAllowance(vmEmployeeSalary vmEmployeeSalary)
        {
            if (vmEmployeeSalary.EmployeeId == null || vmEmployeeSalary.CompanyId == null || vmEmployeeSalary.GrossSalary == null)
                return new VMCommonResult { Id = 0 };

            var computed = await ComputeAsync(vmEmployeeSalary.EmployeeId.Value, vmEmployeeSalary.CompanyId.Value, vmEmployeeSalary.GrossSalary.Value, vmEmployeeSalary.IsPFApplicable ?? true, basicSalary: vmEmployeeSalary.BasicSalary);
            if (computed == null)
                return new VMCommonResult { Id = 0 };

            using var connection = new SqlConnection(_connectionString);

            await connection.ExecuteAsync(@"
                UPDATE EmployeeSalaryAllowance
                SET
                    CompanyId               = @CompanyId,
                    BasicSalary             = @BasicSalary,
                    ChildEducationAllowance = @ChildEducationAllowance,
                    ConveyanceAllowance     = @ConveyanceAllowance,
                    HRA                     = @HRA,
                    MedicalAllowance        = @MedicalAllowance,
                    DeputationAllowance     = @DeputationAllowance,
                    TotalGrossSalary        = @TotalGrossSalary,
                    EmployeePF              = @EmployeePF,
                    EmployeeESI             = @EmployeeESI,
                    ProfessionalTax         = @ProfessionalTax,
                    GroupMedical            = @GroupMedical,
                    TermInsurance           = @TermInsurance,
                    TotalDeductions         = @TotalDeductions,
                    NetSalary               = @NetSalary,
                    EmployerPF              = @EmployerPF,
                    EmployerESI             = @EmployerESI,
                    CTC                     = @CTC
                WHERE EmployeeId = @EmployeeId AND IsDeleted = 0 AND IsEnabled = 1;

                -- Keep the current open-ended EmployeeSalaryHistory row (if any) in sync,
                -- same as USP_CalculateSalaryStructure's UPDATE action used to.
                UPDATE esh
                SET
                    esh.HRA                     = @HRA,
                    esh.ConveyanceAllowance      = @ConveyanceAllowance,
                    esh.ChildEducationAllowance  = @ChildEducationAllowance,
                    esh.MedicalAllowance         = @MedicalAllowance,
                    esh.DeputationAllowance      = @DeputationAllowance,
                    esh.TotalGrossSalary         = @TotalGrossSalary,
                    esh.EmployeePF               = @EmployeePF,
                    esh.EmployeeESI              = @EmployeeESI,
                    esh.ProfessionalTax          = @ProfessionalTax,
                    esh.GroupMedical             = @GroupMedical,
                    esh.TermInsurance            = @TermInsurance,
                    esh.TotalDeductions          = @TotalDeductions,
                    esh.NetSalary                = @NetSalary,
                    esh.EmployerPF               = @EmployerPF,
                    esh.EmployerESI              = @EmployerESI,
                    esh.CTC                      = @CTC
                FROM EmployeeSalaryHistory esh
                WHERE esh.EmployeeId = @EmployeeId
                  AND esh.IsActive = 1
                  AND esh.EffectiveToDate IS NULL;
            ", BuildPersistParameters(vmEmployeeSalary.EmployeeId.Value, vmEmployeeSalary.CompanyId.Value, computed));

            return new VMCommonResult { Id = vmEmployeeSalary.EmployeeId };
        }

        public async Task<VMCommonResult> DeleteEmployeeSalaryAllowance(DeleteRecordVM delete)
        {
            try
            {
                var result = await _db.Set<VMCommonResult>().FromSqlInterpolated($@"
                EXEC USP_CalculateSalaryStructure
                    @Action = {"DELETE"},
                    @Employee = {delete.Id}

            ").ToListAsync();

                return result?.FirstOrDefault() ?? new VMCommonResult { Id = 0 };
            }
            catch
            {
                return new VMCommonResult { Id = 0 };
            }
        }

        public async Task<EmployeeSalaryAllowanceVM?> GetEmployeeSalaryAllowanceByEmployeeId(int EmployeeId)
        {
            try
            {
                var data = await _db.Set<EmployeeSalaryAllowanceVM>().FromSqlInterpolated($"EXEC GetEmployeeSalaryAllowanceByEmployeeId @EmployeeId = {EmployeeId}").ToListAsync();
                return data.FirstOrDefault() ?? null;
            }
            catch
            {
                return null;
            }
        }

        // ------------------------------------------------------------------

        private class ComputedResult
        {
            public AllowanceCalculationResult Allowance { get; set; } = null!;
            public StatutoryDeductionResult Deductions { get; set; } = null!;
        }

        private async Task<ComputedResult?> ComputeAsync(int employeeId, int companyId, decimal grossSalary, bool isPFApplicable, int? gradeIdOverride = null, int? designationIdOverride = null, decimal? basicSalary = null)
        {
            using var connection = new SqlConnection(_connectionString);

            int? gradeId = gradeIdOverride;
            int? designationId = designationIdOverride;

            // Only look the employee row up when the caller didn't already tell us
            // the Grade/Designation — during CREATE (employeeId = 0) there's no row
            // to look up yet, and during live preview the form may have an unsaved
            // Grade change that hasn't reached AspNetUsers yet either.
            if (gradeId == null && designationId == null && employeeId > 0)
            {
                var employee = await connection.QueryFirstOrDefaultAsync<(int? GradeId, int? DesignationId)>(
                    "SELECT GradeId, DesignationId FROM AspNetUsers WHERE Id = @EmployeeId", new { EmployeeId = employeeId });
                gradeId = employee.GradeId;
                designationId = employee.DesignationId;
            }

            var effective = await _templateMappingRepository.GetEffectiveTemplate(employeeId, companyId, gradeId, designationId);
            if (effective?.SalaryStructureTemplateId == null)
                return null;

            var componentsResponse = await _salaryStructureRepository.GetComponentsByTemplateId(effective.SalaryStructureTemplateId.Value);
            var components = componentsResponse.Data as List<SalaryStructureComponent>;
            if (components == null || components.Count == 0)
                return null;

            var allowance = _calculationService.ComputeAllowance(components, grossSalary, manualBasicOverride: basicSalary);

            var statutorySetting = await _statutorySettingRepository.GetByCompanyId(companyId) ?? new CompanyStatutorySetting { CompanyId = companyId };

            var termInsurance = await connection.ExecuteScalarAsync<decimal?>(
                "SELECT dbo.fn_GetTermInsuranceDeduction(@GrossSalary, @ProcessingDate, @CompanyId)",
                new { GrossSalary = grossSalary, ProcessingDate = DateTime.Today, CompanyId = companyId }) ?? 0m;

            var deductions = _calculationService.ComputeStatutoryDeductions(statutorySetting, allowance.BasicSalary, allowance.TotalGrossSalary, grossSalary, isPFApplicable, termInsurance);

            return new ComputedResult { Allowance = allowance, Deductions = deductions };
        }

        private static object BuildPersistParameters(int employeeId, int companyId, ComputedResult computed) => new
        {
            EmployeeId = employeeId,
            CompanyId = companyId,
            computed.Allowance.BasicSalary,
            computed.Allowance.ChildEducationAllowance,
            computed.Allowance.ConveyanceAllowance,
            computed.Allowance.HRA,
            computed.Allowance.MedicalAllowance,
            computed.Allowance.DeputationAllowance,
            computed.Allowance.TotalGrossSalary,
            computed.Deductions.EmployeePF,
            computed.Deductions.EmployeeESI,
            computed.Deductions.ProfessionalTax,
            computed.Deductions.GroupMedical,
            computed.Deductions.TermInsurance,
            computed.Deductions.TotalDeductions,
            NetSalary = computed.Allowance.TotalGrossSalary - computed.Deductions.TotalDeductions,
            computed.Deductions.EmployerPF,
            computed.Deductions.EmployerESI,
            CTC = computed.Allowance.TotalGrossSalary + computed.Deductions.EmployerPF + computed.Deductions.EmployerESI
        };
    }
}
