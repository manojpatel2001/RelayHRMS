using System;

namespace HRMS_Core.Employee
{
    public class EmployeeDesignationHistory
    {
        public int EmployeeDesignationHistoryId { get; set; }
        public int EmployeeId { get; set; }
        public int? OldDesignationId { get; set; }
        public int? NewDesignationId { get; set; }
        public int? OldGradeId { get; set; }
        public int? NewGradeId { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string? Reason { get; set; }
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
