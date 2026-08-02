using System;

namespace HRMS_Core.Recruitment
{
    public class CandidateBGVCheck
    {
        public int CandidateBGVCheckId { get; set; }
        public int CandidateApplicationId { get; set; }
        public string? CheckType { get; set; }
        public string? VendorName { get; set; }
        public string? VendorContact { get; set; }
        public string? Status { get; set; } = "Pending";
        public DateTime? InitiatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public string? ProofDocumentUrl { get; set; }
        public string? Remarks { get; set; }
        public int? VerifiedBy { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
