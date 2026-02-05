using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class PayslipFilterViewModel
    {
        public int EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
