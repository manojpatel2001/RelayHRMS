using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.CompanyInformation
{
    public class vmChangeCompanyLogo
    {
        public int? companyId {  get; set; }    
        public IFormFile? LogoFile {  get; set; }    
        public string? CompanyLogoUrl {  get; set; }    
        public bool? IsDisplayOnLogin {  get; set; }    
    }
}
