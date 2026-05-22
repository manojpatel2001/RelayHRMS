using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class MobileInOutSummaryVM
    {
        public DateTime AttDate { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int BranchId { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public int TotalInPunches { get; set; }
        public int TotalOutPunches { get; set; }
        public decimal WorkingHours { get; set; }
        public string AttendanceStatus { get; set; }
        public string InLocation { get; set; }
        public string OutLocation { get; set; }
        public decimal? InLat { get; set; }
        public decimal? InLong { get; set; }
    }
}
