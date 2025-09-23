using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class EmpInOutReportFilter
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? EmployeeCodes { get; set; }  
        public string? BranchId { get; set; }
        public int CompanyId { get; set; } 
    }
}
