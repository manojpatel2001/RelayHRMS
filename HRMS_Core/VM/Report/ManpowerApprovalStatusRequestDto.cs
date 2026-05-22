using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class ManpowerApprovalStatusRequestDto
    {
        public int ApprovalMasterId { get; set; }
        public int? ManpowerRequisitionId { get; set; }   // null = all records
        public int CompanyId { get; set; }
    }

    public class ApprovalLevelStatusDto
    {
        public int ManpowerRequisitionId { get; set; }
        public int ApprovalRequestId { get; set; }
        public int ApprovalLevelId { get; set; }
        public int LevelNo { get; set; }
        public string? ApproverType { get; set; }
        public int? ApproverEmployeeId { get; set; }
        public string? ApproverName { get; set; }
        public string? ApproverEmail { get; set; }
        public string? LevelStatus { get; set; }
        public int? StatusId { get; set; }
        public string? ActionRemarks { get; set; }
        public int? ActionByEmployeeId { get; set; }
        public string? ActionByName { get; set; }
        public DateTime? ActionOn { get; set; }
        public DateTime? AssignedOn { get; set; }
        public DateTime? EscalationDueOn { get; set; }
        public DateTime? EscalatedOn { get; set; }
        public bool IsCurrentActiveLevel { get; set; }
        public int? DaysPendingAtLevel { get; set; }
        public bool IsOverdue { get; set; }
    }

    // ── HISTORY ──────────────────────────────────────────────────
    public class ApprovalHistoryDto
    {
        public int ManpowerRequisitionId { get; set; }
        public int ApprovalRequestHistoryId { get; set; }
        public int ApprovalRequestId { get; set; }
        public string? ActionType { get; set; }
        public int? ActionByEmployeeId { get; set; }
        public string? ActionByName { get; set; }
        public string? Remarks { get; set; }
        public string? NewStatus { get; set; }
        public DateTime? ActionDate { get; set; }
    }

    // ── MAIN RESPONSE (per Manpower Requisition) ─────────────────
    public class ManpowerApprovalStatusDto
    {
        // Manpower Header
        public int ManpowerRequisitionId { get; set; }
        public string? EmployeeName { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public string? RequirementType { get; set; }
        public int? NumberOfPosition { get; set; }
        public DateTime? RequisitionDate { get; set; }
        public string? RequisitionStatus { get; set; }
        public string? ClosureBy { get; set; }
        public int? RequisitionCreatedBy { get; set; }

        // Approval Request Header
        public int? ApprovalRequestId { get; set; }
        public string? RequestTitle { get; set; }
        public int? CurrentLevelNo { get; set; }
        public DateTime? LastActionDate { get; set; }
        public string? OverallApprovalStatus { get; set; }
        public int? CurrentApproverEmployeeId { get; set; }
        public string? CurrentApproverName { get; set; }
        public int? RequesterEmployeeId { get; set; }
        public string? RequesterName { get; set; }
        public bool IsAdminCreated { get; set; }
        public bool IsAutoApprovedByAdminHR { get; set; }

        // Child collections
        public List<ApprovalLevelStatusDto> Levels { get; set; } = new();
        public List<ApprovalHistoryDto> History { get; set; } = new();
    }
}
