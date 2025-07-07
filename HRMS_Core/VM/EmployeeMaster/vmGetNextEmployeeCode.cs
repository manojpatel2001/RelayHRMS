using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.EmployeeMaster
{
    public class vmGetNextEmployeeCode
    {
        public int NextEmployeeCode { get; set; }
        public string? SampleCode { get; set; }
        public string? DomainName { get; set; }
        public string? DigitsForEmployeeCode { get; set; }
        public string? CompanyCode { get; set; }
    }
}
