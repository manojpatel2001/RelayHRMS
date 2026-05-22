using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class GetAllLeftEmployeeVm
    {
        public int Id { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public string? BranchName { get; set; }

    }
}
