using System;

namespace HRMS_Core.Employee
{
    public class EmployeeWarningHistory
    {
        public int EmployeeWarningHistoryId { get; set; }
        public int EmployeeId { get; set; }
        public int? WarningMasterId { get; set; }
        public DateTime? IssueDate { get; set; }
        public string? IssuedBy { get; set; }
        public string? IncidentDescription { get; set; }
        public string? ActionTaken { get; set; }
        public string Status { get; set; } = "Issued";
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        // Joined from WarningMaster by the read stored procedure — not a real column
        // on this table, so never bound by the CRUD proc's explicit column list.
        public string? WarningName { get; set; }
        public string? Level { get; set; }
    }
}
