using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class TodaysAttendanceAdminViewModel
    {
        public TimeSpan? InTime { get; set; }
        public TimeSpan? OutTime { get; set; }
        public string? AttendanceStatus { get; set; }
        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? ShiftName { get; set; }
        public string? DesignationName { get; set; }
        public string? BranchName { get; set; }
        public string? EmployeeTypeName { get; set; }
        public string? ShiftTime { get; set; }
        public decimal? WorkingHours { get; set; }
        public decimal? SalaryDay { get; set; }
        public string? StatusReason { get; set; }
    }
}
