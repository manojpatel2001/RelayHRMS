using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class GetProbationSearchParam
    {
        public string? SearchBy { get; set; }
        public string? SearchValue { get; set; }
        public string? BranchId { get; set; }
        public string? Status { get; set; }
    }
}
