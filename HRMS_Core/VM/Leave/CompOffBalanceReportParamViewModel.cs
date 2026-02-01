using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class CompOffBalanceReportParamViewModel
    {
        public int? CompId { get; set; }
        public int? EmpId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? AsOnDate { get; set; }
    }
}
