using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class EmployeeInOutReportVM
    {
        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? ShiftTime { get; set; } // e.g., "08:00 to 17:00"
        public string? BranchName { get; set; }
        public DateTime? ShiftDate { get; set; }

        public string? InTime { get; set; }       // Can be "-" or a time string
        public string? OutTime { get; set; }      // Can be "-" or a time string
        public decimal? WorkingHours { get; set; }

        public string? AttendanceStatus { get; set; } // e.g., "P", "A", "HF"
        public decimal? SalaryDay { get; set; }

        public DateTime? CreatedOn { get; set; }
        public string? StatusReason { get; set; }
        public string? MonthDay { get; set; }
        public string? AdjustmentNote { get; set; }
    }
}
