using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class HolidayViewModel
    {
        public string? HolidayName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public bool? RepeatAnnually { get; set; }
        public string? BranchName { get; set; }
    }
}
