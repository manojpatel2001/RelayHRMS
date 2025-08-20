using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class AttendanceDetails
    {
        public DateTime? ShiftDate { get; set; }
        public string? DayName { get; set; }
        public int? AttendanceDetailsId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public decimal? WorkingHours { get; set; }
        public string? AttendanceStatus { get; set; }
        public decimal? SalaryDay { get; set; }
        public DateTime? CreatedOn { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
