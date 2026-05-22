using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public  class LeaveaprovalVM
    {
        public List<int> Ids { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public int? EmployeeId { get; set; }

    }
}
