using System;

namespace HRMS_Core.Recruitment
{
    public class OfferApprovalRequestLevel
    {
        public int OfferApprovalRequestLevelId { get; set; }
        public int OfferApprovalRequestId { get; set; }
        public int LevelNo { get; set; }
        public int? ApproverEmployeeId { get; set; }
        public string? Status { get; set; } = "Pending";
        public string? ActionRemarks { get; set; }
        public int? ActionBy { get; set; }
        public DateTime? ActionOn { get; set; }
        public DateTime? EscalationDueOn { get; set; }
        public DateTime? EscalatedOn { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Joined display fields (pending-approvals + escalation reporting)
        public int? OfferId { get; set; }
        public decimal? OfferedCTC { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string? OfferStatus { get; set; }
        public string? CandidateName { get; set; }
        public string? PositionTitle { get; set; }
        public int? CompanyId { get; set; }
    }
}
