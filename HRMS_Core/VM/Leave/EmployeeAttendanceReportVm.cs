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

        public int? P { get; set; }
        public int? A { get; set; }
        public int? W { get; set; }
        public int? L { get; set; }
        public int? H { get; set; }
    }
}
