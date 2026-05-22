using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class LeaveApp_Param
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Status { get; set; } // Optional: "Pending", "Approved", etc.
        public int? EmpId { get; set; }
        public int? CompId { get; set; }
    }
}
