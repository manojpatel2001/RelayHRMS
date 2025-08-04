using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class vmMyProfile
    {

        public string? FullName { get; set; }
        public string? LoginAlias { get; set; }
        public string? EmployeeCode { get; set; }
        public string? BranchName { get; set; }
        public string? GradeName { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string? ShiftName { get; set; }
        public string? DesignationName { get; set; }
        public string? DepartmentName { get; set; }
        public string? EnrollNo { get; set; }
        public string? EmployeeTypeName { get; set; }
        public string? Email { get; set; }
        public string? CategoryName { get; set; }
        public string? EmployeeProfileUrl { get; set; }
        public string? EmployeeSignatureUrl { get; set; }
        public decimal? MonthlyCTC { get; set; }
        public decimal? MonthlyGross { get; set; }
        public decimal? MonthlyBasic { get; set; }

        // Uncomment if Reporting Manager is required
        // public string ReportingManagerName { get; set; }
    }
}
