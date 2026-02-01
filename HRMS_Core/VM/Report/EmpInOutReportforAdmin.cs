using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class EmpInOutReportforAdmin
    {
        public int EmployeeId { get; set; }
        public string? InTime { get; set; }       // Formatted as "hh:mm tt" (e.g., "09:00 AM")
        public string? OutTime { get; set; }      // Formatted as "hh:mm tt" (e.g., "06:00 PM")
        public decimal? WorkingHours { get; set; }
        public string? AttendanceStatus { get; set; } // "P", "A", "HF", etc.
        public DateTime ShiftDate { get; set; }
        public decimal? SalaryDay { get; set; }
        public string? FullName { get; set; }
        public string? BranchName { get; set; }
        public string? EmployeeCode { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        public string? DesignationName { get; set; }

    }
}
