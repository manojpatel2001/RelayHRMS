using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class ActiveEmployeeDetailsForLetterViewModel
    {
        public string? FullName { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string? FirstName { get; set; }
        public string? Gender { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PermanentAddress { get; set; }
        public string? PermanentPincode { get; set; }
        public string? BranchName { get; set; }
        public string? CTC { get; set; }
        public string? CTCInWords { get; set; }
        public string? ReportingManager { get; set; }
        public string? ReportingManagerGender { get; set; }
        public string? EmployeeCode { get; set; }

        public string? DesignationName { get; set; }
        public string? DepartmentName { get; set; }

        public string? CompanyAddress { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyLogoUrl { get; set; }
        public string? GradeName { get; set; }
        public string? RefNumber { get; set; }

    }
}
