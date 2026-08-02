using System;

namespace HRMS_Core.Recruitment
{
    public class CandidateDocument
    {
        public int CandidateDocumentId { get; set; }
        public int CandidateApplicationId { get; set; }
        public int CandidateDocumentTypeId { get; set; }
        public string? DocumentUrl { get; set; }
        public string? Remarks { get; set; }
        public string? VerifiedStatus { get; set; } = "Pending";
        public int? VerifiedBy { get; set; }
        public DateTime? VerifiedDate { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Joined display fields
        public string? DocumentName { get; set; }
        public bool IsMandatory { get; set; }
    }
}
