using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class SalaryPublishFilterViewModel
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string? EmployeeIds { get; set; } 
        public string? SalaryIds { get; set; } 
        public string? Status { get; set; }
    }
}
