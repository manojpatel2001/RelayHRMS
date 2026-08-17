using System;
using System.Collections.Generic;
using System.Linq;

namespace HRMS_Core.Recruitment
{
    public class AllowanceCalculationResult
    {
        public decimal BasicSalary { get; set; }
        public decimal HRA { get; set; }
        public decimal ConveyanceAllowance { get; set; }
        public decimal ChildEducationAllowance { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal DeputationAllowance { get; set; }
        public decimal TotalGrossSalary { get; set; }

        // Every resolved component, including any beyond the five standard
        // ones above (a template can define extra earnings) — callers that
        // need to persist/display only the standard columns use the named
        // properties; anything wanting the full breakdown uses this list.
        public List<SalaryComponentResolver.ResolvedComponent> Components { get; set; } = new();
    }

    public class StatutoryDeductionResult
    {
        public decimal EmployeePF { get; set; }
        public decimal EmployerPF { get; set; }
        public decimal EmployeeESI { get; set; }
        public decimal EmployerESI { get; set; }
        public decimal ProfessionalTax { get; set; }
        public decimal GroupMedical { get; set; }
        public decimal TermInsurance { get; set; }
        public decimal TotalDeductions { get; set; }
    }

    // Drives Employee Master "Allowance" + Monthly Payroll from a resolved
    // SalaryStructureTemplate's components instead of the hardcoded formulas
    // that used to live in USP_CalculateSalaryStructure / USP_CalculateMonthlySalary_V2
    // (`Basic = 40% of Gross`, flat 1600/200/1250, `IF @CompanyId = 11`, literal
    // EmployeeId 165/218 overrides). Earning-side amounts come from the
    // template's components (reusing SalaryComponentResolver, the same engine
    // Offer/Recruitment fitment already uses via SalaryFitmentService in
    // HRMS_API); statutory-deduction-side amounts come from
    // CompanyStatutorySetting instead of literals, so a company needing
    // different PF/ESI/PT/GroupMedical rules is a data change, not a code
    // change. Term Insurance still comes from the existing
    // fn_GetTermInsuranceDeduction SQL function — callers fetch that value via
    // the repository layer and pass it in here, keeping this service DB-free.
    // Lives in HRMS_Core (not HRMS_API) because it's called from
    // HRMS_Infrastructure repositories, which cannot reference HRMS_API.
    public class SalaryStructureCalculationService
    {
        public AllowanceCalculationResult ComputeAllowance(List<SalaryStructureComponent> templateComponents, decimal grossSalary, decimal prorationFactor = 1.0m, decimal? manualBasicOverride = null)
        {
            // roundingDecimals: 0 — matches USP_CalculateSalaryStructure exactly,
            // which rounds every calculated allowance figure to whole rupees.
            var resolved = SalaryComponentResolver.Resolve(templateComponents, grossSalary, prorationFactor, manualBasicOverride, roundingDecimals: 0);
            var result = new AllowanceCalculationResult { Components = resolved };

            foreach (var component in resolved)
            {
                switch (NormalizeName(component.ComponentName))
                {
                    case "BASIC":
                        result.BasicSalary = component.Amount;
                        break;
                    case "HRA":
                        result.HRA = component.Amount;
                        break;
                    case "CONVEYANCE":
                    case "CONVEYANCEALLOWANCE":
                        result.ConveyanceAllowance = component.Amount;
                        break;
                    case "CHILDEDUCATION":
                    case "CHILDEDUCATIONALLOWANCE":
                        result.ChildEducationAllowance = component.Amount;
                        break;
                    case "MEDICAL":
                    case "MEDICALALLOWANCE":
                        result.MedicalAllowance = component.Amount;
                        break;
                    case "DEPUTATION":
                    case "DEPUTATIONALLOWANCE":
                        result.DeputationAllowance = component.Amount;
                        break;
                }
            }

            result.TotalGrossSalary = resolved.Sum(c => c.Amount);
            return result;
        }

        // earnedBasic/earnedGrossTotal are the (possibly prorated) allowance
        // amounts just computed above; baseGrossSalary is the employee's
        // un-prorated master Gross Salary — thresholds like the ESI/GroupMedical
        // ceiling apply against the base figure, matching today's SP.
        public StatutoryDeductionResult ComputeStatutoryDeductions(
            CompanyStatutorySetting setting,
            decimal earnedBasic,
            decimal earnedGrossTotal,
            decimal baseGrossSalary,
            bool isPFApplicable,
            decimal termInsuranceAmount)
        {
            var result = new StatutoryDeductionResult();

            if (setting.IsPFEnabled && isPFApplicable)
            {
                var pf = Math.Round(earnedBasic * (setting.PFPercentage / 100m), 0);
                result.EmployeePF = Math.Min(pf, setting.PFCapAmount);
                result.EmployerPF = result.EmployeePF;
            }

            if (setting.IsESIEnabled && baseGrossSalary <= setting.ESIGrossCeiling)
            {
                result.EmployeeESI = Math.Round(earnedGrossTotal * (setting.ESIPercentage / 100m), 0);
                result.EmployerESI = Math.Round(baseGrossSalary * (setting.EmployerESIPercentage / 100m), 0);
            }

            if (setting.IsProfessionalTaxEnabled && earnedGrossTotal > setting.ProfessionalTaxThreshold)
                result.ProfessionalTax = setting.ProfessionalTaxAmount;

            if (setting.IsGroupMedicalEnabled && baseGrossSalary >= setting.GroupMedicalGrossThreshold)
                result.GroupMedical = setting.GroupMedicalAmount;

            if (setting.IsTermInsuranceEnabled)
                result.TermInsurance = termInsuranceAmount;

            result.TotalDeductions = result.EmployeePF + result.EmployeeESI + result.ProfessionalTax + result.GroupMedical + result.TermInsurance;
            return result;
        }

        private static string NormalizeName(string? componentName) =>
            (componentName ?? string.Empty).Replace(" ", "").ToUpperInvariant();
    }
}
