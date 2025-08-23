using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Ess.InOut
{
    public class vmGetMonthlyAttendanceLog
    {
        public int? EmployeeId { get; set; }
        public string? FullName { get; set; }
        public string? BranchName { get; set; }
        public string? EmployeeCode { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? Mode { get; set; }
        public DateTime? PunchDateTime { get; set; }
        public string? PunchTime { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
    }
}
