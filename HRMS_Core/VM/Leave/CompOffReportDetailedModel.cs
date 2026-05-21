using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class CompOffReportDetailedModel
    {
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public DateTime? CompoffApplicationDate { get; set; }
        public DateTime? ApprovalExtraWorkedDate { get; set; }
        public string? WorkedHour { get; set; }
        public DateTime? LeaveAdjustedDate { get; set; }
        public string? LeaveAdjustedPeriod { get; set; }
    }
}
