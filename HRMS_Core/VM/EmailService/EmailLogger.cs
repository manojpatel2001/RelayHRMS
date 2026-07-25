using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.EmailService
{
    public class EmailLogger
    {
        public int Id { get; set; }
        public string? FromEmail { get; set; }
        public string? ToEmail { get; set; }
        public string? BCCEmail { get; set; }
        public string? CCEmail { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
        public string? Status { get; set; }
        public DateTime? SentAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? AttachmentsUrl { get; set; }
        public string? Comments { get; set; }
    }

   

}
