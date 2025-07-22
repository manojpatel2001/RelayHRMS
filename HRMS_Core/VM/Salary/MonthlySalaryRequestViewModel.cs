using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class MonthlySalaryRequestViewModel
    {

        public DateTime StartDate { get; set; }   
        public DateTime EndDate { get; set; }     
        public string? EmployeeCodes { get; set; } 
        public int? BranchId { get; set; }
        public string? Action { get; set; }
    }
}
