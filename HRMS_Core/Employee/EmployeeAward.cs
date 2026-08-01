using System;

namespace HRMS_Core.Employee
{
    public class EmployeeAward
    {
        public int EmployeeAwardId { get; set; }
        public int EmployeeId { get; set; }
        public string AwardName { get; set; } = string.Empty;
        public string? AwardCategory { get; set; }
        public DateTime? AwardDate { get; set; }
        public string? AwardedBy { get; set; }
        public string? Description { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
