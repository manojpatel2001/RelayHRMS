using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ApprovalManagement
{
    public class ApprovalSchemeLevelVM
    {
        public int? ApprovalSchemeLevelId { get; set; }

        public int SchemeId { get; set; }
        public int SequenceNo { get; set; }
        public int? ApproverEmployeeId { get; set; }
        public string? ApproverDesignationName { get; set; }
        public bool IsDepartmentBased { get; set; }
        public int? EscalationDays { get; set; }
        public int? SkipDays { get; set; }
        public bool IsActive { get; set; }
        public bool IsNotMandatory { get; set; }

        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? DeletedBy { get; set; }
        public int? CompanyId { get; set; }
        public List<ApprovalSchemeLevelDepartmentVM>? approvalSchemeLevelDepartmentVMs { get; set; }
       
    }

    public class ApprovalSchemeLevelDepartmentVM
    {
        public int? ApprovalSchemeLevelId { get; set; }
        public int? DepartmentId { get; set; }
        public int? DeptApproverEmployeeId { get; set; }
        public string? DeptApproverDesignationName { get; set; }
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
    public class GetApprovalSchemeLevelVM
    {
        public int ApprovalSchemeLevelId { get; set; }
        public int SchemeId { get; set; }
        public string? SchemeName { get; set; }
        public int SequenceNo { get; set; }
        public int? ApproverEmployeeId { get; set; }
        public string? ApproverName { get; set; }
        public string? ApproverDesignationName { get; set; }
        public bool IsDepartmentBased { get; set; }
        public bool IsNotMandatory { get; set; }
        public int? EscalationDays { get; set; }
        public int? SkipDays { get; set; }
        public bool IsActive { get; set; }
        public int CompanyId { get; set; }

        // This property is used to temporarily hold the JSON string from SQL
        public string? approvalSchemeLevelDepartmentVMsJson { get; set; }

        // This is the actual property used in your application
        public List<GetApprovalSchemeLevelDepartmentVM>? approvalSchemeLevelDepartmentVMs { get; set; }
    }

    public class GetApprovalSchemeLevelDepartmentVM
    {
        public int ApprovalSchemeLevelDeptId { get; set; }
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public int? DeptApproverEmployeeId { get; set; }
        public string? DeptApproverName { get; set; }
        public string? DeptApproverDesignationName { get; set; }
    }
    public class ApprovalSchemeLevelPara
    {
        public int SchemeId { get; set; }
        public int ApprovalSchemeLevelId { get; set; }
        public int DeletedBy { get; set; }
    }


}
