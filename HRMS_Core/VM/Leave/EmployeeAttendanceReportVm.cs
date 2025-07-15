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

        public string BranchName { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }

        [NotMapped]
        public IDictionary<string, object> Days { get; set; } = new ExpandoObject();

        public int? Total_P { get; set; }
        public int? Total_A { get; set; }
        public int? Total_W { get; set; }
        public int? Total_HF { get; set; }
    }
}
