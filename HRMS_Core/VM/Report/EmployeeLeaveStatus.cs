using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class EmployeeLeaveStatus
    {
        public string? LeaveType { get; set; }
        public decimal OpeningBalance { get; set; }
        public decimal Credit { get; set; }
        public decimal Used { get; set; }
        public decimal ClosingBalance { get; set; }
    }
}
