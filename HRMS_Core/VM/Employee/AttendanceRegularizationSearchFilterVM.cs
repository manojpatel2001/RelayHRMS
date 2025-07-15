using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class AttendanceRegularizationSearchFilterVM
    {
        public string? SearchBy { get; set; }        // "EmpName", "EmpCode", or "Reason"
        public string? SearchValue { get; set; }     // Text value to search
        public DateTime? FromDate { get; set; }     // Optional From Date
        public DateTime? ToDate { get; set; }       // Optional To Date
        public string? Status { get; set; }
    }
}
