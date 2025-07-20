using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.EmployeeMaster
{
    public class AttachmentDetails
    {
        public int AttachmentDetailsId { set; get; }
        public int? EmployeeId { set; get; }
        public string? DocumentName { set; get; }
        public string? DocumentUrl { set; get; }
        public string? Comment { set; get; }
        public DateTime? DateOfExpiry { set; get; }

        public bool? IsDeleted { get; set; } = false;
        public bool? IsEnabled { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }
    
}
