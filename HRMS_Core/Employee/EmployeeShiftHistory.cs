using System;

namespace HRMS_Core.Employee
{
    public class EmployeeShiftHistory
    {
        public int EmployeeShiftHistoryId { get; set; }
        public int EmployeeId { get; set; }
        public int? OldShiftMasterId { get; set; }
        public int? NewShiftMasterId { get; set; }
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
