using System;

namespace HRMS_Core.Recruitment
{
    public class EmployeeSalaryStructureTemplate
    {
        public int EmployeeSalaryStructureTemplateId { get; set; }
        public int EmployeeId { get; set; }
        public int SalaryStructureTemplateId { get; set; }
        public int CompanyId { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Joined display field
        public string? TemplateName { get; set; }
    }

    // Result shape of GetEffectiveSalaryStructureTemplate — the resolver that
    // an employee's explicit mapping (ResolvedFrom = "Employee") or their
    // Grade/Designation default template (ResolvedFrom = "GradeDefault"),
    // or neither (SalaryStructureTemplateId = null, unconfigured).
    public class EffectiveSalaryStructureTemplateResult
    {
        public int? SalaryStructureTemplateId { get; set; }
        public string? ResolvedFrom { get; set; }
    }
}
