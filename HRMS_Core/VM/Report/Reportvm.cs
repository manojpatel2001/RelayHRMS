using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class Reportvm
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string BranchId { get; set; }   // "12,15,20"
        public string EmpId { get; set; }
    }
}
