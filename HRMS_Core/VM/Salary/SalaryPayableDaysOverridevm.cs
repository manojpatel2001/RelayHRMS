using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class SalaryPayableDaysOverridevm
    {
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public int MonthNumber { get; set; }
        public int Year { get; set; }
        public int OverridePayableDays { get; set; }
        public int AdjustmentDelta { get; set; }
        public string Reason { get; set; }
        public string CreatedByName { get; set; }
        public string BranchName { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
