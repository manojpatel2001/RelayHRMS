using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class CompoffLeaveBalanceViewModel
    {
        public int TotalBalance { get; set; }
        public int PendingLeaves { get; set; }
        public int AvailableBalance { get; set; }
        public DateTime LastBalanceDate { get; set; }
        public string StatusMessage { get; set; }
    }
}
