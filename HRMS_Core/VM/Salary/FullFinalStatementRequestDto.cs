using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Salary
{
    public class FullFinalStatementRequestDto
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public string? EmployeeCode { get; set; }
    }
}
