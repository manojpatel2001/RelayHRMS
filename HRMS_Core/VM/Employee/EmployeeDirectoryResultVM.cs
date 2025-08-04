using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class EmployeeDirectoryResultVM
    {

        public string? EmployeeProfileUrl { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public string? Gender { get; set; }
        public string? BranchName { get; set; }
        public string? DepartmentName { get; set; }
        public string? DesignationName { get; set; }
        public DateTime? DateOfJoining { get; set; }
    }
}
