using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class NewJoinerDetailsViewModel
    {
        public int EmpID { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public string? DepartmentName { get; set; }
        public string? DesignationName { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string? BranchName { get; set; }
    }
}
