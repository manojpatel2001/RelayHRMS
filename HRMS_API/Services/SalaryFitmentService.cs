using HRMS_Core.Recruitment;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRMS_API.Services
{
    // Self-contained CTC Builder calculation — applies a SalaryStructureTemplate's
    // component rules (%-of-CTC, %-of-Basic, flat amount) against a Final CTC to produce
    // the offer's salary breakup. Rule resolution itself lives in
    // HRMS_Core.Recruitment.SalaryComponentResolver, shared with
    // SalaryStructureCalculationService (Employee Master allowance + Monthly Payroll,
    // called from HRMS_Infrastructure) so both engines stay in lockstep — it lives in
    // HRMS_Core rather than here because HRMS_Infrastructure cannot reference HRMS_API.
    // Does not touch Earning/Deduction/payroll.
    public class SalaryFitmentService
    {
        public List<OfferSalaryBreakupComponent> ComputeBreakup(List<SalaryStructureComponent> templateComponents, decimal finalCTC)
        {
            var resolved = SalaryComponentResolver.Resolve(templateComponents, finalCTC);
            return resolved.Select(BuildBreakupRow).ToList();
        }

        private static OfferSalaryBreakupComponent BuildBreakupRow(SalaryComponentResolver.ResolvedComponent component)
        {
            // Bonus-category components (Joining Bonus, Retention Bonus, annual Variable Pay)
            // are conventionally annual figures; everything else is monthly.
            var frequency = string.Equals(component.ComponentCategory, "Bonus", StringComparison.OrdinalIgnoreCase) ? "Annual" : "Monthly";

            return new OfferSalaryBreakupComponent
            {
                ComponentName = component.ComponentName,
                ComponentCategory = component.ComponentCategory,
                Frequency = frequency,
                Amount = component.Amount
            };
        }
    }
}
