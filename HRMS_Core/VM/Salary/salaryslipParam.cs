using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class salaryslipParam
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int EmployeeId { get; set; }

    }

    public class salaryslipParamReport
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public List<int> EmployeeId { get; set; }

    }
}