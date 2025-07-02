using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.EmployeeMaster
{
    public class vmUpdateEmployeeProfile
    {
        public int? EmployeeId {  get; set; }
        public IFormFile? EmployeeProfileFile {  get; set; }
        public IFormFile? EmployeeSignatureFile {  get; set; }
        public string? EmployeeProfileUrl { get; set; }
        public string? EmployeeSignatureUrl { get; set; }
    }
}
