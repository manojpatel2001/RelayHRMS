using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class ApproveandrejectVM
    {
        public List<int> CompoffIds { get; set; }
        public string Status { get; set; }
        public int? EmployeeId { get; set; }
    }
}
