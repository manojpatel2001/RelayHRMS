using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class LeaveApplicationReportModel
    {
        public int? LeaveApplicationId { get; set; }
        public string? LeaveTypeName { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? ReportingPersonName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? NoOfDays { get; set; } 
        public string? Reason { get; set; }
        public string? LeaveStatus { get; set; }
    }
}
