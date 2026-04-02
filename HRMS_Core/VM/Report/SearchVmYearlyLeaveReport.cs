using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class SearchVmYearlyLeaveReport
    {
        public string? EmpIds { get; set; }  // comma separated e.g. "1,2,3" or null for all
        public int? CompanyId { get; set; }
        public int? BranchId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}
