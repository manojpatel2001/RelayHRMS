using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class LeaveBalanceViewModel
    {
        public string? LeaveType { get; set; }
        public decimal? LeaveValue { get; set; }
        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? CompanyName { get; set; }
    }
}
