using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class AttendanceLockParamVm
    {
        public int? Month { get; set; }
        public int? Year { get; set; }
        public string? EmployeeId { get; set; }
        public string? Status { get; set; }
    }
}
