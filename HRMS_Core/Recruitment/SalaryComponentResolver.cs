using System;
using System.Collections.Generic;
using System.Linq;

namespace HRMS_Core.Recruitment
{
    // Shared two-pass SalaryStructureComponent resolution, factored out of
    // SalaryFitmentService (Offer/Recruitment fitment, in HRMS_API.Services)
    // so SalaryStructureCalculationService (Employee Master allowance +
    // Monthly Payroll, called from HRMS_Infrastructure) can reuse the exact
    // same rule engine instead of re-implementing it. Lives in HRMS_Core
    // (not HRMS_API) specifically because HRMS_Infrastructure does not — and
    // must not — reference HRMS_API; HRMS_Core is the dependency-free
    // project both sides already reference. The only two things that differ
    // between callers are the base amount (FinalCTC for fitment, GrossSalary
    // for allowance/payroll) and whether a proration factor applies (payroll
    // only).
    public static class SalaryComponentResolver
    {
        public class ResolvedComponent
        {
            public string? ComponentName { get; set; }
            public string? ComponentCategory { get; set; }
            public string? CalculationType { get; set; }
            public decimal Amount { get; set; }
        }

        // Pass 1: PercentOfCTC / FlatAmount (captures the row named "Basic").
        // Pass 2: PercentOfBasic, using the Basic amount captured in pass 1.
        // Pass 3: RemainderOfGross — whatever's left of baseAmount after every
        // other enabled component. Can legitimately be negative (see the note
        // in the loop below) — not floored at 0. At most one such component is
        // expected per template; if more than one exists each gets the same
        // full remainder (template authoring error, not something to silently
        // half-guess about).
        // manualBasicOverride mirrors the old USP_CalculateSalaryStructure /
        // USP_CalculateMonthlySalary_V2 semantics: "if Basic Salary was
        // manually entered (non-zero), use it as-is instead of computing it
        // from the template's Basic component" — HR routinely types a Basic
        // Salary directly on Employee Master that doesn't match the template's
        // formula, and a recalculation must not silently overwrite it.
        // roundingDecimals defaults to 2 to preserve SalaryFitmentService's
        // original, already-shipped Offer/Recruitment behavior unchanged.
        // SalaryStructureCalculationService (Employee Master + Payroll) passes
        // 0 explicitly — verified against the live OLD USP_CalculateSalaryStructure,
        // every single ROUND(...) call there rounds to whole rupees, and a
        // real employee (Gross=34742, Basic=4928) exposed the mismatch:
        // HRA = 40% of 4928 = 1971.20 at 2 decimals vs. the old SP's 1971.00.
        public static List<ResolvedComponent> Resolve(List<SalaryStructureComponent> templateComponents, decimal baseAmount, decimal prorationFactor = 1.0m, decimal? manualBasicOverride = null, int roundingDecimals = 2)
        {
            var result = new List<ResolvedComponent>();
            bool basicManuallySet = manualBasicOverride is > 0;
            decimal basicAmount = basicManuallySet ? manualBasicOverride!.Value : 0;
            decimal runningTotal = 0;

            var ordered = templateComponents
                .Where(c => c.IsEnabled && !c.IsDeleted)
                .OrderBy(c => c.SortOrder)
                .ToList();

            foreach (var component in ordered.Where(c => !IsPercentOfBasic(c) && !IsRemainderOfGross(c)))
            {
                bool isBasicComponent = string.Equals(component.ComponentName, "Basic", StringComparison.OrdinalIgnoreCase);

                decimal amount;
                if (isBasicComponent && basicManuallySet)
                {
                    amount = basicAmount;
                }
                else
                {
                    amount = component.CalculationType?.ToUpperInvariant() switch
                    {
                        "PERCENTOFCTC" => Math.Round(baseAmount * (component.Value / 100m), roundingDecimals),
                        "FLATAMOUNT" => Math.Round(component.Value * prorationFactor, roundingDecimals),
                        _ => 0m
                    };

                    if (isBasicComponent)
                        basicAmount = amount;
                }

                runningTotal += amount;
                result.Add(BuildRow(component, amount));
            }

            foreach (var component in ordered.Where(IsPercentOfBasic))
            {
                var amount = Math.Round(basicAmount * (component.Value / 100m), roundingDecimals);
                runningTotal += amount;
                result.Add(BuildRow(component, amount));
            }

            foreach (var component in ordered.Where(IsRemainderOfGross))
            {
                // Deliberately NOT floored at 0 — this mirrors the old SP's
                // Deputation Allowance exactly, which is a genuine balancing
                // plug and can legitimately go negative when the other fixed
                // components (often a manually-entered Basic Salary) exceed
                // Gross. Flooring here would silently break the invariant that
                // Basic + HRA + Conveyance + ChildEdu + Medical + Deputation
                // == Gross, which real employee data actually relies on.
                var amount = Math.Round(baseAmount - runningTotal, roundingDecimals);
                result.Add(BuildRow(component, amount));
            }

            return result;
        }

        private static bool IsPercentOfBasic(SalaryStructureComponent c) =>
            string.Equals(c.CalculationType, "PercentOfBasic", StringComparison.OrdinalIgnoreCase);

        private static bool IsRemainderOfGross(SalaryStructureComponent c) =>
            string.Equals(c.CalculationType, "RemainderOfGross", StringComparison.OrdinalIgnoreCase);

        private static ResolvedComponent BuildRow(SalaryStructureComponent component, decimal amount) => new()
        {
            ComponentName = component.ComponentName,
            ComponentCategory = component.ComponentCategory,
            CalculationType = component.CalculationType,
            Amount = amount
        };
    }
}
