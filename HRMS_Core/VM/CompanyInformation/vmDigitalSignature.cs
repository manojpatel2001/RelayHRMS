using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.CompanyInformation
{
    public class vmDigitalSignature
    {
        public int? CompanyId { get; set; }
        public IFormFile? DigitalSignatureFile { get; set; }
        public bool? IsDigitalSignature { get; set; }
        public string? DigitalSignatureUrl { get; set; }
        public string? DigitalSignaturePassword { get; set; }
    }
}
