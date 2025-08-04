using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class RecentEmployeeVM
    {
        public string? FullName { get; set; }
        public string? EmployeeProfileUrl { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string? DesignationName { get; set; }
    }
}
