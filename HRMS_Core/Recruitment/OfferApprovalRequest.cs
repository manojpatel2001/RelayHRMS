using System;

namespace HRMS_Core.Recruitment
{
    public class OfferApprovalRequest
    {
        public int OfferApprovalRequestId { get; set; }
        public int OfferId { get; set; }
        public int CurrentLevelNo { get; set; } = 1;
        public string? Status { get; set; } = "Pending";
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
