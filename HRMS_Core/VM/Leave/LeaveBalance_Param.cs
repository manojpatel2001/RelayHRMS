using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class LeaveBalance_Param
    {

        public DateTime? AsOfDate { get; set; }
        public int? Status { get; set; }
        public int? EmpId { get; set; }
        public int? CompId { get; set; }

    }

    public class LeaveBalance_ParamForAdmin
    {
        public DateTime? AsOfDate { get; set; }
        public int? Status { get; set; }
        public List<string> EmployeeCodes { get; set; }
        public int? CompId { get; set; }

    }
}
