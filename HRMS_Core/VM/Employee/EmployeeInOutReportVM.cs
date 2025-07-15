using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class EmployeeInOutReportVM
    {
        public DateTime For_Date { get; set; }
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? BranchName { get; set; }
        public DateTime? In_Time { get; set; }
        public DateTime? Out_Time { get; set; }
        public string? ShiftTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public string? Status { get; set; }
    }
}
