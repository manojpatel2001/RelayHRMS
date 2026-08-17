using System.Collections.Generic;
using HRMS_Core.Recruitment;
using Xunit;

namespace HRMS_Core.Tests.Recruitment
{
    public class SalaryComponentResolverTests
    {
        private static SalaryStructureComponent Component(string name, string category, string calcType, decimal value, int sortOrder = 0, bool isEnabled = true, bool isDeleted = false) => new()
        {
            ComponentName = name,
            ComponentCategory = category,
            CalculationType = calcType,
            Value = value,
            SortOrder = sortOrder,
            IsEnabled = isEnabled,
            IsDeleted = isDeleted
        };

        [Fact]
        public void Resolve_PercentOfCTC_ComputesAgainstBaseAmount()
        {
            var components = new List<SalaryStructureComponent>
            {
                Component("Special Allowance", "Earning", "PercentOfCTC", 32.00m, 10)
            };

            var result = SalaryComponentResolver.Resolve(components, baseAmount: 81000m);

            Assert.Single(result);
            Assert.Equal(25920m, result[0].Amount); // 81000 * 32%
        }

        [Fact]
        public void Resolve_PercentOfBasic_UsesBasicResolvedInPass1_RegardlessOfSortOrder()
        {
            // "Basic" is sorted AFTER "HRA" here — the two-pass design must still
            // resolve Basic first internally so HRA's %-of-Basic calc is correct.
            var components = new List<SalaryStructureComponent>
            {
                Component("HRA", "Earning", "PercentOfBasic", 50.00m, 10),
                Component("Basic", "Earning", "PercentOfCTC", 40.00m, 20)
            };

            var result = SalaryComponentResolver.Resolve(components, baseAmount: 81000m);

            var basic = result.Find(c => c.ComponentName == "Basic");
            var hra = result.Find(c => c.ComponentName == "HRA");

            Assert.Equal(32400m, basic!.Amount); // 81000 * 40%
            Assert.Equal(16200m, hra!.Amount);   // 32400 * 50%
        }

        [Theory]
        [InlineData(1.0, 1600)]     // full month — flat amount unchanged
        [InlineData(0.5, 800)]      // half the payable days — prorated
        [InlineData(0.0, 0)]        // zero payable days
        public void Resolve_FlatAmount_AppliesProrationFactor(decimal factor, decimal expected)
        {
            var components = new List<SalaryStructureComponent>
            {
                Component("Conveyance", "Earning", "FlatAmount", 1600m)
            };

            var result = SalaryComponentResolver.Resolve(components, baseAmount: 50000m, prorationFactor: factor);

            Assert.Equal(expected, result[0].Amount);
        }

        [Fact]
        public void Resolve_RemainderOfGross_TakesWhateverIsLeftAfterOtherComponents()
        {
            var components = new List<SalaryStructureComponent>
            {
                Component("Basic", "Earning", "PercentOfCTC", 40.00m, 10),
                Component("HRA", "Earning", "PercentOfBasic", 40.00m, 20),
                Component("Deputation", "Earning", "RemainderOfGross", 0m, 30)
            };

            // Basic = 40000, HRA = 40% of 40000 = 16000, Deputation = 100000 - 40000 - 16000 = 44000
            var result = SalaryComponentResolver.Resolve(components, baseAmount: 100000m);

            var deputation = result.Find(c => c.ComponentName == "Deputation");
            Assert.Equal(44000m, deputation!.Amount);
        }

        [Fact]
        public void Resolve_RemainderOfGross_CanGoNegative_MatchesOldSPDeputationBehavior()
        {
            // Verified against the live OLD USP_CalculateSalaryStructure for a
            // real employee (Gross=24067, manually-entered Basic=17225): its
            // Deputation Allowance came back as -3098.00, a genuine balancing
            // plug, not floored at 0. This must NOT be clamped, or
            // Basic+HRA+Conveyance+ChildEdu+Medical+Deputation no longer
            // reconciles exactly back to Gross.
            var components = new List<SalaryStructureComponent>
            {
                Component("Basic", "Earning", "FlatAmount", 17225m, 10),
                Component("HRA", "Earning", "PercentOfBasic", 40.00m, 20),
                Component("Conveyance", "Earning", "FlatAmount", 1600m, 30),
                Component("Child Education Allowance", "Earning", "FlatAmount", 200m, 40),
                Component("Medical Allowance", "Earning", "FlatAmount", 1250m, 50),
                Component("Deputation", "Earning", "RemainderOfGross", 0m, 60)
            };

            var result = SalaryComponentResolver.Resolve(components, baseAmount: 24067m);

            var deputation = result.Find(c => c.ComponentName == "Deputation");
            Assert.Equal(-3098m, deputation!.Amount);

            decimal total = 0m;
            foreach (var c in result) total += c.Amount;
            Assert.Equal(24067m, total); // reconciles exactly back to Gross
        }

        [Fact]
        public void Resolve_ExcludesDisabledAndDeletedComponents()
        {
            var components = new List<SalaryStructureComponent>
            {
                Component("Basic", "Earning", "PercentOfCTC", 40.00m, 10),
                Component("Disabled Allowance", "Earning", "FlatAmount", 5000m, 20, isEnabled: false),
                Component("Deleted Allowance", "Earning", "FlatAmount", 5000m, 30, isDeleted: true)
            };

            var result = SalaryComponentResolver.Resolve(components, baseAmount: 100000m);

            Assert.Single(result);
            Assert.Equal("Basic", result[0].ComponentName);
        }

        [Fact]
        public void Resolve_UnrecognizedCalculationType_ResolvesToZero_NotExcluded()
        {
            var components = new List<SalaryStructureComponent>
            {
                Component("Mystery Component", "Earning", "SomeFutureType", 999m, 10)
            };

            var result = SalaryComponentResolver.Resolve(components, baseAmount: 100000m);

            Assert.Single(result);
            Assert.Equal(0m, result[0].Amount);
        }

        [Fact]
        public void Resolve_EmptyComponentList_ReturnsEmptyResult()
        {
            var result = SalaryComponentResolver.Resolve(new List<SalaryStructureComponent>(), baseAmount: 50000m);

            Assert.Empty(result);
        }

        [Fact]
        public void Resolve_BasicNameMatch_IsCaseInsensitive()
        {
            var components = new List<SalaryStructureComponent>
            {
                Component("HRA", "Earning", "PercentOfBasic", 50.00m, 10),
                Component("BASIC", "Earning", "PercentOfCTC", 40.00m, 20) // uppercase
            };

            var result = SalaryComponentResolver.Resolve(components, baseAmount: 81000m);

            var hra = result.Find(c => c.ComponentName == "HRA");
            Assert.Equal(16200m, hra!.Amount); // still resolves against the uppercase "BASIC" row
        }

        [Fact]
        public void Resolve_ManualBasicOverride_ReplacesTemplateFormula_ForBasicRowAndDependents()
        {
            // Matches USP_CalculateSalaryStructure semantics: a manually-entered
            // Basic Salary (here 35670, vs. 40% of 140625 = 56250 the template
            // would otherwise compute) must win, and HRA (%-of-Basic) must be
            // calculated off the manual value, not the template's own formula.
            var components = new List<SalaryStructureComponent>
            {
                Component("Basic", "Earning", "PercentOfCTC", 40.0000m, 10),
                Component("HRA", "Earning", "PercentOfBasic", 40.0000m, 20)
            };

            var result = SalaryComponentResolver.Resolve(components, baseAmount: 140625m, manualBasicOverride: 35670m);

            decimal Amount(string name) => result.Find(c => c.ComponentName == name)!.Amount;

            Assert.Equal(35670m, Amount("Basic"));
            Assert.Equal(14268m, Amount("HRA")); // 40% of 35670
        }

        [Fact]
        public void Resolve_ManualBasicOverride_Zero_IsTreatedAsNotProvided_UsesTemplateFormula()
        {
            // 0 means "no manual Basic entered yet" (matches the old SP's
            // `IF @BasicSalary = 0` fallback), not "Basic should literally be 0".
            var components = new List<SalaryStructureComponent>
            {
                Component("Basic", "Earning", "PercentOfCTC", 40.0000m, 10)
            };

            var result = SalaryComponentResolver.Resolve(components, baseAmount: 140625m, manualBasicOverride: 0m);

            Assert.Equal(56250m, result[0].Amount); // 40% of 140625, formula still applies
        }

        [Fact]
        public void Resolve_NoManualBasicOverride_UsesTemplateFormula()
        {
            var components = new List<SalaryStructureComponent>
            {
                Component("Basic", "Earning", "PercentOfCTC", 40.0000m, 10)
            };

            var result = SalaryComponentResolver.Resolve(components, baseAmount: 140625m);

            Assert.Equal(56250m, result[0].Amount);
        }

        [Fact]
        public void Resolve_RealWorldTemplate_MatchesLiveVerifiedFigures()
        {
            // Exact components/values from live SalaryStructureTemplateId=1, verified
            // end-to-end against the running app for EmployeeId=14 (Gross=81000):
            // Basic=32400, HRA=16200, Special Allowance=25920, Employer PF=3888,
            // Joining Bonus=10000 -> TotalGrossSalary=88408.
            var components = new List<SalaryStructureComponent>
            {
                Component("Basic", "Earning", "PercentOfCTC", 40.0000m, 10),
                Component("HRA", "Earning", "PercentOfBasic", 50.0000m, 20),
                Component("Special Allowance", "Earning", "PercentOfCTC", 32.0000m, 30),
                Component("Employer PF", "Statutory", "PercentOfBasic", 12.0000m, 40),
                Component("Joining Bonus", "Bonus", "FlatAmount", 10000.0000m, 50)
            };

            var result = SalaryComponentResolver.Resolve(components, baseAmount: 81000m);

            decimal Amount(string name) => result.Find(c => c.ComponentName == name)!.Amount;

            Assert.Equal(32400m, Amount("Basic"));
            Assert.Equal(16200m, Amount("HRA"));
            Assert.Equal(25920m, Amount("Special Allowance"));
            Assert.Equal(3888m, Amount("Employer PF"));
            Assert.Equal(10000m, Amount("Joining Bonus"));

            decimal total = 0m;
            foreach (var c in result) total += c.Amount;
            Assert.Equal(88408m, total);
        }
    }
}
