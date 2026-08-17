using System.Collections.Generic;
using HRMS_Core.Recruitment;
using Xunit;

namespace HRMS_Core.Tests.Recruitment
{
    public class SalaryStructureCalculationServiceTests
    {
        private readonly SalaryStructureCalculationService _service = new();

        private static SalaryStructureComponent Component(string name, string category, string calcType, decimal value, int sortOrder = 0) => new()
        {
            ComponentName = name,
            ComponentCategory = category,
            CalculationType = calcType,
            Value = value,
            SortOrder = sortOrder,
            IsEnabled = true,
            IsDeleted = false
        };

        private static CompanyStatutorySetting StandardSetting(int companyId = 9) => new()
        {
            CompanyId = companyId,
            IsPFEnabled = true,
            PFPercentage = 12.00m,
            PFCapAmount = 1800.00m,
            IsESIEnabled = true,
            ESIPercentage = 0.75m,
            EmployerESIPercentage = 3.25m,
            ESIGrossCeiling = 21000.00m,
            IsProfessionalTaxEnabled = true,
            ProfessionalTaxThreshold = 12000.00m,
            ProfessionalTaxAmount = 200.00m,
            IsGroupMedicalEnabled = true,
            GroupMedicalGrossThreshold = 21000.00m,
            GroupMedicalAmount = 266.00m,
            IsTermInsuranceEnabled = true
        };

        // ---------------------------------------------------------------
        // ComputeAllowance
        // ---------------------------------------------------------------

        [Theory]
        [InlineData("Conveyance")]
        [InlineData("Conveyance Allowance")]
        public void ComputeAllowance_MapsConveyanceVariants_ToConveyanceAllowance(string componentName)
        {
            var components = new List<SalaryStructureComponent> { Component(componentName, "Earning", "FlatAmount", 1600m) };

            var result = _service.ComputeAllowance(components, grossSalary: 50000m);

            Assert.Equal(1600m, result.ConveyanceAllowance);
        }

        [Fact]
        public void ComputeAllowance_MapsAllSixStandardNames_ToTheirNamedProperties()
        {
            var components = new List<SalaryStructureComponent>
            {
                Component("Basic", "Earning", "FlatAmount", 20000m),
                Component("HRA", "Earning", "FlatAmount", 8000m),
                Component("Conveyance Allowance", "Earning", "FlatAmount", 1600m),
                Component("Child Education Allowance", "Earning", "FlatAmount", 200m),
                Component("Medical Allowance", "Earning", "FlatAmount", 1250m),
                Component("Deputation Allowance", "Earning", "FlatAmount", 5000m)
            };

            var result = _service.ComputeAllowance(components, grossSalary: 50000m);

            Assert.Equal(20000m, result.BasicSalary);
            Assert.Equal(8000m, result.HRA);
            Assert.Equal(1600m, result.ConveyanceAllowance);
            Assert.Equal(200m, result.ChildEducationAllowance);
            Assert.Equal(1250m, result.MedicalAllowance);
            Assert.Equal(5000m, result.DeputationAllowance);
        }

        [Fact]
        public void ComputeAllowance_UnrecognizedComponentName_StillCountsTowardTotalGross()
        {
            var components = new List<SalaryStructureComponent>
            {
                Component("Basic", "Earning", "FlatAmount", 20000m),
                Component("Special Allowance", "Earning", "FlatAmount", 5000m) // not one of the 6 named properties
            };

            var result = _service.ComputeAllowance(components, grossSalary: 50000m);

            Assert.Equal(20000m, result.BasicSalary);
            Assert.Equal(0m, result.ConveyanceAllowance); // untouched
            Assert.Equal(25000m, result.TotalGrossSalary); // but included in the total
        }

        [Fact]
        public void ComputeAllowance_PercentageComponents_RoundToWholeRupees_NotTwoDecimals()
        {
            // Regression: live parity check against USP_CalculateSalaryStructure
            // for a real employee (Gross=34742, manually-entered Basic=4928)
            // showed HRA = ROUND(40% of 4928, 0) = 1971 in the old SP, but this
            // engine originally rounded to 2 decimals (1971.20) instead. Every
            // ROUND(...) in the old SP uses 0 decimal places.
            var components = new List<SalaryStructureComponent>
            {
                Component("Basic", "Earning", "PercentOfCTC", 40.00m, 10),
                Component("HRA", "Earning", "PercentOfBasic", 40.00m, 20)
            };

            var result = _service.ComputeAllowance(components, grossSalary: 34742m, manualBasicOverride: 4928m);

            Assert.Equal(4928m, result.BasicSalary);
            Assert.Equal(1971m, result.HRA); // not 1971.20
        }

        [Fact]
        public void ComputeAllowance_NoComponents_ReturnsAllZero()
        {
            var result = _service.ComputeAllowance(new List<SalaryStructureComponent>(), grossSalary: 50000m);

            Assert.Equal(0m, result.BasicSalary);
            Assert.Equal(0m, result.TotalGrossSalary);
            Assert.Empty(result.Components);
        }

        // ---------------------------------------------------------------
        // ComputeStatutoryDeductions — PF
        // ---------------------------------------------------------------

        [Fact]
        public void ComputeStatutoryDeductions_PF_CapsAtConfiguredAmount()
        {
            // 12% of 30000 = 3600, which exceeds the 1800 cap.
            var result = _service.ComputeStatutoryDeductions(StandardSetting(), earnedBasic: 30000m, earnedGrossTotal: 50000m, baseGrossSalary: 50000m, isPFApplicable: true, termInsuranceAmount: 0m);

            Assert.Equal(1800m, result.EmployeePF);
            Assert.Equal(1800m, result.EmployerPF);
        }

        [Fact]
        public void ComputeStatutoryDeductions_PF_BelowCap_UsesPercentageAmount()
        {
            // 12% of 10000 = 1200, under the cap.
            var result = _service.ComputeStatutoryDeductions(StandardSetting(), earnedBasic: 10000m, earnedGrossTotal: 15000m, baseGrossSalary: 15000m, isPFApplicable: true, termInsuranceAmount: 0m);

            Assert.Equal(1200m, result.EmployeePF);
        }

        [Fact]
        public void ComputeStatutoryDeductions_PF_DisabledForCompany_ReturnsZeroEvenIfEmployeeIsApplicable()
        {
            var setting = StandardSetting();
            setting.IsPFEnabled = false;

            var result = _service.ComputeStatutoryDeductions(setting, earnedBasic: 30000m, earnedGrossTotal: 50000m, baseGrossSalary: 50000m, isPFApplicable: true, termInsuranceAmount: 0m);

            Assert.Equal(0m, result.EmployeePF);
            Assert.Equal(0m, result.EmployerPF);
        }

        [Fact]
        public void ComputeStatutoryDeductions_PF_NotApplicableForEmployee_ReturnsZeroEvenIfCompanyEnablesIt()
        {
            var result = _service.ComputeStatutoryDeductions(StandardSetting(), earnedBasic: 30000m, earnedGrossTotal: 50000m, baseGrossSalary: 50000m, isPFApplicable: false, termInsuranceAmount: 0m);

            Assert.Equal(0m, result.EmployeePF);
        }

        // ---------------------------------------------------------------
        // ComputeStatutoryDeductions — ESI
        // ---------------------------------------------------------------

        [Fact]
        public void ComputeStatutoryDeductions_ESI_ZeroWhenBaseGrossAboveCeiling()
        {
            var result = _service.ComputeStatutoryDeductions(StandardSetting(), earnedBasic: 32400m, earnedGrossTotal: 88408m, baseGrossSalary: 81000m, isPFApplicable: false, termInsuranceAmount: 0m);

            Assert.Equal(0m, result.EmployeeESI);
            Assert.Equal(0m, result.EmployerESI);
        }

        [Fact]
        public void ComputeStatutoryDeductions_ESI_AppliesDifferentEmployeeAndEmployerPercentages()
        {
            // Base gross (20000) is under the 21000 ceiling, so ESI applies.
            var result = _service.ComputeStatutoryDeductions(StandardSetting(), earnedBasic: 8000m, earnedGrossTotal: 20000m, baseGrossSalary: 20000m, isPFApplicable: false, termInsuranceAmount: 0m);

            Assert.Equal(150m, result.EmployeeESI);  // 0.75% of 20000
            Assert.Equal(650m, result.EmployerESI);  // 3.25% of 20000 (base gross, not earned-gross-total)
        }

        // ---------------------------------------------------------------
        // ComputeStatutoryDeductions — Professional Tax / Group Medical / Term Insurance
        // ---------------------------------------------------------------

        [Theory]
        [InlineData(12001, 200)]
        [InlineData(12000, 0)] // threshold is exclusive ("> threshold", not ">=")
        [InlineData(5000, 0)]
        public void ComputeStatutoryDeductions_ProfessionalTax_GatedByThreshold(decimal earnedGross, decimal expectedPT)
        {
            var result = _service.ComputeStatutoryDeductions(StandardSetting(), earnedBasic: 0m, earnedGrossTotal: earnedGross, baseGrossSalary: earnedGross, isPFApplicable: false, termInsuranceAmount: 0m);

            Assert.Equal(expectedPT, result.ProfessionalTax);
        }

        [Theory]
        [InlineData(21000, 266)] // threshold is inclusive (">=")
        [InlineData(20999, 0)]
        public void ComputeStatutoryDeductions_GroupMedical_GatedByThreshold(decimal baseGross, decimal expectedGroupMedical)
        {
            var result = _service.ComputeStatutoryDeductions(StandardSetting(), earnedBasic: 0m, earnedGrossTotal: baseGross, baseGrossSalary: baseGross, isPFApplicable: false, termInsuranceAmount: 0m);

            Assert.Equal(expectedGroupMedical, result.GroupMedical);
        }

        [Fact]
        public void ComputeStatutoryDeductions_TermInsurance_PassesThroughTheProvidedAmountWhenEnabled()
        {
            var result = _service.ComputeStatutoryDeductions(StandardSetting(), earnedBasic: 0m, earnedGrossTotal: 5000m, baseGrossSalary: 5000m, isPFApplicable: false, termInsuranceAmount: 450m);

            Assert.Equal(450m, result.TermInsurance);
        }

        [Fact]
        public void ComputeStatutoryDeductions_TermInsurance_ZeroWhenDisabled_EvenIfAmountIsProvided()
        {
            var setting = StandardSetting();
            setting.IsTermInsuranceEnabled = false;

            var result = _service.ComputeStatutoryDeductions(setting, earnedBasic: 0m, earnedGrossTotal: 5000m, baseGrossSalary: 5000m, isPFApplicable: false, termInsuranceAmount: 450m);

            Assert.Equal(0m, result.TermInsurance);
        }

        [Fact]
        public void ComputeStatutoryDeductions_TotalDeductions_SumsAllComponents()
        {
            var result = _service.ComputeStatutoryDeductions(StandardSetting(), earnedBasic: 32400m, earnedGrossTotal: 88408m, baseGrossSalary: 81000m, isPFApplicable: false, termInsuranceAmount: 450m);

            // PF=0 (not applicable), ESI=0 (above ceiling), PT=200, GroupMedical=266, TermInsurance=450
            Assert.Equal(916m, result.TotalDeductions);
        }

        [Fact]
        public void ComputeStatutoryDeductions_Company11StyleSetting_MatchesLiveSeededConfiguration()
        {
            // Mirrors the actual seeded CompanyStatutorySetting row for CompanyId=11
            // (PF/ESI/ProfessionalTax disabled; GroupMedical/TermInsurance still apply) —
            // this is the config that replaces the old `IF @CompanyId = 11` SQL branch.
            var company11Setting = new CompanyStatutorySetting
            {
                CompanyId = 11,
                IsPFEnabled = false,
                IsESIEnabled = false,
                IsProfessionalTaxEnabled = false,
                IsGroupMedicalEnabled = true,
                GroupMedicalGrossThreshold = 21000.00m,
                GroupMedicalAmount = 266.00m,
                IsTermInsuranceEnabled = true
            };

            var result = _service.ComputeStatutoryDeductions(company11Setting, earnedBasic: 50000m, earnedGrossTotal: 50000m, baseGrossSalary: 50000m, isPFApplicable: true, termInsuranceAmount: 100m);

            Assert.Equal(0m, result.EmployeePF);
            Assert.Equal(0m, result.EmployeeESI);
            Assert.Equal(0m, result.ProfessionalTax);
            Assert.Equal(266m, result.GroupMedical);
            Assert.Equal(100m, result.TermInsurance);
            Assert.Equal(366m, result.TotalDeductions);
        }
    }
}
