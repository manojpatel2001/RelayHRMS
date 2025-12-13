using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class vmLeaveCancellationReportFilter
    {
        public string? SearchBy { get; set; } // e.g., "EmpCode", "EmpName"
        public string? SearchValue { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? LeaveStatus { get; set; } // "Pending", "Approved", "Rejected"
        public int EmployeeId { get; set; }
    }
}
