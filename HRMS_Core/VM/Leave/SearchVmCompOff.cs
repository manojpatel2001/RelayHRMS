using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class SearchVmCompOff
    {
        public string? SearchType { get; set; }         // "EmployeeName" or "EmployeeCode"
        public string? SearchFor { get; set; }          // search text
        public string? Status { get; set; }             // "Pending", "Approved", "Rejected"
        public DateTime? ExtraWorkDate { get; set; }    // opt
    }
}
