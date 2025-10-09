namespace HRMS_Core.EmployeeMaster
{
    public class vmGetAttachmentDetailById
    {
        public int? AttachmentDetailsId { set; get; }
        public int? EmployeeId { set; get; }
        public string? DocumentName { set; get; }
        public int? DocumentTypeId { set; get; }
        public string? DocumentUrl { set; get; }
        public string? Comment { set; get; }
        public DateTime? DateOfExpiry { set; get; }

        public DateTime CreatedDate { get; set; } 
        
        
    }
    
}
