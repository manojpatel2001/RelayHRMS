using System;

namespace HRMS_Core.Recruitment
{
    public class OfferApprovalRequestHistory
    {
        public int OfferApprovalRequestHistoryId { get; set; }
        public int OfferApprovalRequestId { get; set; }
        public int OfferApprovalRequestLevelId { get; set; }
        public string? ActionType { get; set; }
        public int? ActionBy { get; set; }
        public string? Remarks { get; set; }
        public string? OldStatus { get; set; }
        public string? NewStatus { get; set; }
        public DateTime ActionDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
    }
}
