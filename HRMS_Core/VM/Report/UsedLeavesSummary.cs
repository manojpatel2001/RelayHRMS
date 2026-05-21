using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class UsedLeavesSummary
    {
        public string? LeaveName { get; set; }  
        public decimal? TotalUsedDays { get; set; }
    }
}
