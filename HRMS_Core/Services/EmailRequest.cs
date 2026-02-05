using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Services
{
    public class EmailRequest
    {
        public List<string> ToEmails { get; set; } = new();
        public List<string>? CcEmails { get; set; }
        public List<string>? BccEmails { get; set; }
        public string? AttachmentPaths { get; set; }
        public string Subject { get; set; } = string.Empty;
        public string TemplateName { get; set; } = string.Empty;
        public Dictionary<string, string>? Placeholders { get; set; }
        public byte[]? PdfFile { get; set; }
    }

}
