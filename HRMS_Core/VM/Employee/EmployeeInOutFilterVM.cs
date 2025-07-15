using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class EmployeeInOutFilterVM
    {
        public int? BranchId { get; set; }
        public int? EmpId { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public string? RecordType { get; set; }
    }
}
