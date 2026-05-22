using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class MonthlySalarySummaryViewModel
    {
        public string? MonthYear { get; set; }  
        public int? MonthNumber { get; set; }   
        public decimal? TotalNetSalary { get; set; }
    }
}
