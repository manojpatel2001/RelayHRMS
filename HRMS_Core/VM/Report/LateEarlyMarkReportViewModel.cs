using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class LateEarlyMarkReportViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime ShiftDate { get; set; }
        public string ShiftName { get; set; }
        public TimeSpan ShiftStartTime { get; set; }
        public TimeSpan ShiftEndTime { get; set; }
        public TimeSpan? ActualInTime { get; set; }
        public TimeSpan? ActualOutTime { get; set; }
        public string LateStatus { get; set; }
        public int LateByMinutes { get; set; }
        public string EarlyLeaveStatus { get; set; }
        public int EarlyByMinutes { get; set; }
        public string Remarks { get; set; }
        public string AttendanceStatus { get; set; }
        public decimal WorkingHours { get; set; }
    }
}
