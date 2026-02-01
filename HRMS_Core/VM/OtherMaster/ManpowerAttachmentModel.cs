using Microsoft.AspNetCore.Http;

namespace HRMS_Core.VM.OtherMaster
{
    public class ManpowerAttachmentModel
    {
        public int? ManpowerAttachmentId { get; set; }
        public int? ManpowerRequisitionId { get; set; }
        public int? DocumentTypeId { get; set; }
        public string? DocumentUrl { get; set; }
        public IFormFile? DocumentFile { get; set; }
        public string? Comment { get; set; }
        public DateTime? DateOfExpiry { get; set; }
        public bool? IsEnabled { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
    }
    public class vmManpowerAttachment
    {
        public int? ManpowerAttachmentId { get; set; }
        public int? ManpowerRequisitionId { get; set; }
        public int? DocumentTypeId { get; set; }
        public string? DocumentUrl { get; set; }
        public string? Comment { get; set; }
        public DateTime? DateOfExpiry { get; set; }
        public bool? IsEnabled { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
    }

}
