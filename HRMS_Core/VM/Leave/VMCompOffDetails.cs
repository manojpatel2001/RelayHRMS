using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class VMCompOffDetails
    {
        public int? Comp_Off_Detailsid { get; set; }
        public string? Emp_Code { get; set; }
        public DateTime? Extra_Work_Day { get; set; }
        public string? Extra_Work_Hours { get; set; }
        public string? ComoffReason { get; set; }
        public string? Application_Status { get; set; }
        public string? FullName { get; set; }
        public string? BranchName { get; set; }
    }
}
