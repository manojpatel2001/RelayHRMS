using System;

namespace HRMS_Core.Recruitment
{
    public class SalaryStructureComponent
    {
        public int SalaryStructureComponentId { get; set; }
        public int SalaryStructureTemplateId { get; set; }
        public string? ComponentName { get; set; }
        public string? ComponentCategory { get; set; }
        public string? CalculationType { get; set; }
        public decimal Value { get; set; }
        public int SortOrder { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
