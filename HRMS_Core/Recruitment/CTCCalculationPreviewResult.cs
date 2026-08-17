using System.Collections.Generic;

namespace HRMS_Core.Recruitment
{
    // Response shape for the "CTC Calculation Preview" admin page — lets HR
    // pick any Salary Structure Template directly (no employee/grade
    // resolution involved) and see exactly what it would produce for a given
    // Gross Salary, before assigning it to anyone. Same underlying engine as
    // Employee Master's live allowance preview (SalaryStructureCalculationService),
    // just addressed by an explicit TemplateId instead of a resolved one.
    public class CTCCalculationPreviewResult
    {
        public decimal BasicSalary { get; set; }
        public decimal HRA { get; set; }
        public decimal ConveyanceAllowance { get; set; }
        public decimal ChildEducationAllowance { get; set; }
        public decimal MedicalAllowance { get; set; }
        public decimal DeputationAllowance { get; set; }
        public decimal TotalGrossSalary { get; set; }

        public decimal EmployeePF { get; set; }
        public decimal EmployerPF { get; set; }
        public decimal EmployeeESI { get; set; }
        public decimal EmployerESI { get; set; }
        public decimal ProfessionalTax { get; set; }
        public decimal GroupMedical { get; set; }
        public decimal TermInsurance { get; set; }
        public decimal TotalDeductions { get; set; }

        public decimal NetSalary { get; set; }
        public decimal CTC { get; set; }

        // Every resolved component exactly as the template defines it —
        // including any beyond the six standard named fields above (a
        // template can have custom earnings/bonus rows), so the reviewer
        // sees the template's actual full output, not just what fits the
        // Employee Master form's fixed columns.
        public List<SalaryComponentResolver.ResolvedComponent> Components { get; set; } = new();
    }
}
