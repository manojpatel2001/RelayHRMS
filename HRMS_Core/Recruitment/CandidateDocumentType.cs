using System;

namespace HRMS_Core.Recruitment
{
    public class CandidateDocumentType
    {
        public int CandidateDocumentTypeId { get; set; }
        public string? DocumentName { get; set; }
        public bool IsMandatory { get; set; } = false;
        public int SortOrder { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
