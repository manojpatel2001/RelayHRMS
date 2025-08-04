using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.importData
{
    public class EmpAttendanceVM
    {
        public int? EmpAttendanceId { get; set; }
        public string? Att_Detail { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public string? BranchName { get; set; }
    }
}
