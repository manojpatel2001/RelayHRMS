using HRMS_Core.Recruitment;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HRMS_API.Services
{
    // Self-contained CTC Builder calculation — applies a SalaryStructureTemplate's
    // component rules (%-of-CTC, %-of-Basic, flat amount) against a Final CTC to produce
    // the offer's salary breakup. Pure C# calculation, no business logic in SQL, matching
    // Phase 1's ResumeScreeningService philosophy. Does not touch Earning/Deduction/payroll.
    public class SalaryFitmentService
    {
        public List<OfferSalaryBreakupComponent> ComputeBreakup(List<SalaryStructureComponent> templateComponents, decimal finalCTC)
        {
            var result = new List<OfferSalaryBreakupComponent>();
            decimal basicAmount = 0;

            var ordered = templateComponents.OrderBy(c => c.SortOrder).ToList();

            // Pass 1: components whose amount doesn't depend on Basic (PercentOfCTC / FlatAmount).
            foreach (var component in ordered.Where(c => !string.Equals(c.CalculationType, "PercentOfBasic", StringComparison.OrdinalIgnoreCase)))
            {
                decimal amount = component.CalculationType?.ToUpperInvariant() switch
                {
                    "PERCENTOFCTC" => Math.Round(finalCTC * (component.Value / 100m), 2),
                    "FLATAMOUNT" => component.Value,
                    _ => 0m
                };

                if (string.Equals(component.ComponentName, "Basic", StringComparison.OrdinalIgnoreCase))
                    basicAmount = amount;

                result.Add(BuildBreakupRow(component, amount));
            }

            // Pass 2: components computed as a % of the already-resolved Basic.
            foreach (var component in ordered.Where(c => string.Equals(c.CalculationType, "PercentOfBasic", StringComparison.OrdinalIgnoreCase)))
            {
                var amount = Math.Round(basicAmount * (component.Value / 100m), 2);
                result.Add(BuildBreakupRow(component, amount));
            }

            return result;
        }

        private static OfferSalaryBreakupComponent BuildBreakupRow(SalaryStructureComponent component, decimal amount)
        {
            // Bonus-category components (Joining Bonus, Retention Bonus, annual Variable Pay)
            // are conventionally annual figures; everything else is monthly.
            var frequency = string.Equals(component.ComponentCategory, "Bonus", StringComparison.OrdinalIgnoreCase) ? "Annual" : "Monthly";

            return new OfferSalaryBreakupComponent
            {
                ComponentName = component.ComponentName,
                ComponentCategory = component.ComponentCategory,
                Frequency = frequency,
                Amount = amount
            };
        }
    }
}
