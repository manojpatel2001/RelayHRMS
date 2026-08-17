using System;

namespace HRMS_Core.Recruitment
{
    public class SalaryStructureTemplate
    {
        public int SalaryStructureTemplateId { get; set; }
        public string? TemplateName { get; set; }
        public int? GradeId { get; set; }
        public int? DesignationId { get; set; }
        public int? CompanyId { get; set; }
        public bool IsDefault { get; set; } = false;
        public string? Status { get; set; } = "Active";
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Joined display fields
        public string? GradeName { get; set; }
        public string? DesignationName { get; set; }
        public string? CompanyName { get; set; }
    }
}
