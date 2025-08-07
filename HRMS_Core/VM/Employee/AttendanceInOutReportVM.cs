using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class AttendanceInOutReportVM
    {
        public DateTime? For_Date { get; set; }
        public string? Day_Name { get; set; }

        public string? In_Time { get; set; }
        public string? Out_Time { get; set; }

        public string? Duration { get; set; }

        public int? Emp_Id { get; set; }

        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? BranchName { get; set; }
    }

    public class EmpInOutVM
    {
        public int EmployeeId { get; set; }
        public DateTime For_Date { get; set; }
        public DateTime? In_Time { get; set; }
        public DateTime? Out_Time { get; set; }
        public int Id { get; set; }
    }

}
