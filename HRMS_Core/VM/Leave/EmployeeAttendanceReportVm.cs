using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class EmployeeAttendanceReportVm
    {

        public string? BranchName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }

        [NotMapped]
        public IDictionary<string, object> Days { get; set; } = new ExpandoObject();

        public string? P { get; set; }
        public string? A { get; set; }
        public string? W { get; set; }
        public string? L { get; set; }
        public string? H { get; set; }
    }
}
