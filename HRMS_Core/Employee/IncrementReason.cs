using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Employee
{
    public class IncrementReason
    {
        public int? ReasonId { get; set; }
        public string? ReasonName { get; set; }
        public bool? IsActive { get; set; }
    }

}
