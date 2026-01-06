using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Probations
{
    public class ProbationEmployeeVM
    {
        public int? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? ProbationEndDate { get; set; }
        public string? ReviewType { get; set; }
        public int? ExtendedInDays { get; set; }
        public DateTime? EvaluationDate { get; set; }
        public string? BranchName { get; set; }
    }


    public class GetPendingApprovalRequestsWithHistoryPara
    {
        public int? ApproverEmployeeId { get; set; }
        public int StatusId { get; set; } = 1;
    }
    public class GetPendingApprovalRequestsWithHistoryPara1
    {
        public int? ApproverEmployeeId { get; set; }
        public int StatusId { get; set; } = 1;
        public int ApprovalMasterId { get; set; }
    }


    public class PendingApprovalRequestwithHistrory
    {
        public int ApprovalRequestId { get; set; }
        public string RequestTitle { get; set; }

        public int RequestStatusId { get; set; }
        public string RequestStatus { get; set; }

        public int CurrentApprovalRequestLevelId { get; set; }
        public int CurrentLevelNo { get; set; }

        public int CurrentLevelStatusId { get; set; }
        public string CurrentLevelStatus { get; set; }

        public DateTime AssignedOn { get; set; }
        public DateTime? EscalationDueOn { get; set; }

        public int RequesterEmployeeId { get; set; }
        public string RequesterName { get; set; }

      
        public int ApproverEmployeeId { get; set; }
        public string ApproverName { get; set; }

        public string PreviousApprovalLevelsJson { get; set; }
        public List<ApprovalLevelHistory> PreviousApprovalLevels { get; set; }
    }
    public class PendingApprovalRequestwithHistrory1
    {
        public int ApprovalRequestId { get; set; }
        public string RequestTitle { get; set; }

        public int RequestStatusId { get; set; }
        public string RequestStatus { get; set; }

        public int CurrentApprovalRequestLevelId { get; set; }
        public int CurrentLevelNo { get; set; }

        public int CurrentLevelStatusId { get; set; }
        public string CurrentLevelStatus { get; set; }

        public DateTime AssignedOn { get; set; }
        public DateTime? EscalationDueOn { get; set; }

        public int RequesterEmployeeId { get; set; }
        public string RequesterName { get; set; }

      
        public int ApproverEmployeeId { get; set; }
        public string ApproverName { get; set; }
        public int NoOfInstallment { get; set; }
        public decimal InstallmentAmount { get; set; }
        public DateTime LoanRequireDate { get; set; }
        public decimal LoanAmount { get; set; }
        public string LoanName { get; set; }
        public int LoanApplicationID { get; set; }
        public string PreviousApprovalLevelsJson { get; set; }
        public List<ApprovalLevelHistory> PreviousApprovalLevels { get; set; }
    }

    public class ApprovalLevelHistory
    {
        public int HistoryApprovalRequestLevelId { get; set; }
        public int HistoryLevelNo { get; set; }

        public int HistoryStatusId { get; set; }
        public string HistoryStatus { get; set; }

        public int HistoryApproverEmployeeId { get; set; }
        public string HistoryApproverName { get; set; }

        public DateTime AssignedOn { get; set; }
        public DateTime? EscalationDueOn { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }


    public class GetAllConfirmationProbationDetailsPara
    {
        public int CompanyId { get; set; }
        public int? StatusId { get; set; }
        public int? EmployeeId { get; set; }
        public int ApprovalMasterId { get; set; }

    }

    public class ConfirmationProbationDetails
    {
        public int? EmployeeId { get; set; }
        public int ApprovalRequestId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PersonalEmailId { get; set; }
        public string? CompanyName { get; set; }
        public int? StatusId { get; set; }
        public string? StatusName { get; set; }
        public DateTime? ActionDate { get; set; }
        public DateTime? ProbationEndDate { get; set; }
        public string? Location { get; set; }
        public bool IsMailSent { get; set; } = false;
        public string? ReportingManagerName { get; set; }
        public string? ReportingMangerId { get; set; }
        public string? ReportingManagerCode { get; set; }
        public string? ReportingManagerEmail { get; set; }
        public int? CurrentLevelNo { get; set; }
        public string? CurrentApprover { get; set; }

    }
    public class ConfirmationProbationDetailsPara
    {
        public int? EmployeeId { get; set; }
        public int ApprovalRequestId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? PersonalEmailId { get; set; }
        public string? ReportingManagerEmail { get; set; }
        public string? CompanyName { get; set; }
        public string? StatusName { get; set; }
        public DateTime? ActionDate { get; set; }
        public string? Location { get; set; }
       public string ConfirmationPdf { get; set; }
        public bool IsMailSent { get; set; } = false;

    }


}
