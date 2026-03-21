using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class SalaryPayableDaysPara
    {
        public int? MonthNumber { get; set; }
        public int? Year { get; set; }
        public string? EmployeeCodes { get; set; }
        public int? BranchId { get; set; }
        public string? AdjustmentType { get; set; }
    }
}
