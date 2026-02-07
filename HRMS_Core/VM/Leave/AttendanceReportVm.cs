using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class AttendanceReportVm
    {

        public List<int>? EmployeeIds { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class AttendanceReportforAdminVm
    {
        public List<int>? EmployeeIds { get; set; }
        public List<int>? BranchIds { get; set; }  // New property for branch filtering
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
