using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class LeaveTypevm
    {
        public string? Leave { get; set; }
        public decimal? Opening { get; set; }
        public decimal? Credit { get; set; }
        public decimal? Used { get; set; }
        public decimal? Balance { get; set; }
        public decimal? PendingLeaves { get; set; }

    }
}
