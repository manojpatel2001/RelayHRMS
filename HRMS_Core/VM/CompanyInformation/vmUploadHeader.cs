using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.CompanyInformation
{
    public class vmUploadHeader
    {
        public int? companyId { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public IFormFile? LetterHeadHeaderFile { get; set; }
        public IFormFile? LetterHeadFooterFile { get; set; }

        public DateTime? EffectiveDate { get; set; }
        public string? LetterHeadHeaderUrl { get; set; }
        public string? LetterHeadFooterUrl { get; set; }
    }
}
