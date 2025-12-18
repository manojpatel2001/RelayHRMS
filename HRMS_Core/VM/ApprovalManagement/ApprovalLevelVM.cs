using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ApprovalManagement
{
    public class ApprovalLevelVM
    {
        public int? ApprovalLevelId { get; set; }
        public int ApprovalMasterId { get; set; }
        public int LevelNo { get; set; }
        public int? ApproverEmployeeId { get; set; }
        public bool IsReportingPerson { get; set; }
        public bool IsHR { get; set; }
        public bool IsNationalManager { get; set; }
        public bool IsHOD { get; set; }
        public bool IsDepartmentBased { get; set; }
        public int EscalationDays { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public int? CompanyId { get; set; }
        public List<ApprovalLevelDepartmentVM>? ApprovalLevelDepartments { get; set; }
    }

    public class ApprovalLevelDepartmentVM
    {
        public int? ApprovalLevelId { get; set; }
        public int DepartmentId { get; set; }
        public int? ApproverEmployeeId { get; set; }
        public bool IsNotMandatory { get; set; }
    }

    
    public class ApprovalRequestVM
    {
        public string Action { get; set; }
        public int? ApprovalRequestId { get; set; }

        public int ApprovalSchemeId { get; set; }
        public int RequesterEmployeeId { get; set; }
        public int? RequesterDepartmentId { get; set; }

        public string RequestTitle { get; set; }
        public string? RequestData { get; set; }

        public int? CurrentLevelSeq { get; set; }
        public string Status { get; set; }  // Pending / Approved / Rejected

        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
    }

    public class ApprovalRequestLevelVM
    {
        public string Action { get; set; }
        public int? ApprovalRequestLevelId { get; set; }

        public int ApprovalRequestId { get; set; }
        public int ApprovalSchemeLevelId { get; set; }
        public int SequenceNo { get; set; }
        public int? ApproverEmployeeId { get; set; }

        public string? Status { get; set; }
        public string? ActionRemarks { get; set; }
        public int? ActionBy { get; set; }
        public DateTime? ActionOn { get; set; }

        public DateTime? EscalatedOn { get; set; }
        public DateTime? EscalationDueOn { get; set; }

        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
    }
    public class ApprovalRequestHistoryVM
    {
        public int ApprovalRequestId { get; set; }
        public int ApprovalRequestLevelId { get; set; }

        public string ActionType { get; set; }  // Approve/Reject/Escalate
        public int ActionBy { get; set; }
        public string? Remarks { get; set; }

        public string? OldStatus { get; set; }
        public string? NewStatus { get; set; }

        public int CreatedBy { get; set; }
    }

    public class ApprovalRequestLevelActionVM
    {
        public int ApprovalRequestId { get; set; }
        public int ApprovalSchemeLevelId { get; set; }

        public string Action { get; set; } // Approve or Reject
        public int ActionBy { get; set; }
        public string? Remarks { get; set; }
    }



    // Helper class to map the JSON string from SQL
    public class GetApprovalLevelVM
    {
        public int ApprovalLevelId { get; set; }
        public int ApprovalMasterId { get; set; }
        public string? ApprovalName { get; set; }
        public int LevelNo { get; set; }
        public int? ApproverEmployeeId { get; set; }
        public string? ApproverName { get; set; }
        public bool IsReportingPerson { get; set; }
        public bool IsHOD { get; set; }
        public bool IsNationalManager { get; set; }
        public bool IsHR { get; set; }
        public bool IsDepartmentBased { get; set; }
        public int? EscalationDays { get; set; }
        public int CompanyId { get; set; }
        public string? approvalLevelDepartmentVMsJson { get; set; }
        public List<GetApprovalLevelDepartmentVM>? approvalLevelDepartmentVMs { get; set; }
    }

    public class GetApprovalLevelDepartmentVM
    {
        public int ApprovalLevelDeptId { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? DeptApproverEmployeeId { get; set; }
        public string? DeptApproverName { get; set; }
        public bool IsNotMandatory { get; set; }
    }

    public class ApprovalLevelPara
    {
        public int ApprovalMasterId { get; set; }
        public int ApprovalLevelId { get; set; }
        public int DeletedBy { get; set; }
    }


    public class ApprovalRequestLevelActionPara
    {
        public int ApprovalRequestId { get; set; }
        public int ApprovalRequestLevelId { get; set; }
        public int StatusId { get; set; }
        public int ActionBy { get; set; }
        public string Remarks { get; set; }
    }
    public class GetPendingApprovalRequestsPara
    {
        public int? ApproverEmployeeId { get; set; }
        public int StatusId { get; set; }
    }

    public class PendingApprovalRequest
    {
        public int ApprovalRequestId { get; set; }
        public string RequestTitle { get; set; }
        public string RequestData { get; set; }
        public string RequestStatus { get; set; }
        public int ApprovalRequestLevelId { get; set; }
        public int SequenceNo { get; set; }
        public string LevelStatus { get; set; }
        public int ApproverEmployeeId { get; set; }
        public int RequesterEmployeeId { get; set; }
        public DateTime? EscalationDueOn { get; set; }
        public DateTime? AssignedOn { get; set; }
        public string ApproverName { get; set; }
        public string RequesterName { get; set; }
        public int? CurrentLevelSequence { get; set; }
    }

    public class GetUpcomingProbationDetailsPara
    {
        public int? EmployeeId { get; set; }
    }
    public class ApprovalRequestLevelActionVm
    {
        public bool IsAllLevelsCompleted { get; set; }
        public bool IsSuccess { get; set; }
        public string? ResponseMessage { get; set; }
    }
}
