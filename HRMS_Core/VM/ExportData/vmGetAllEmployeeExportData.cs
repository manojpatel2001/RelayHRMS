using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ExportData
{
    public class vmGetAllEmployeeExportData
    {
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public string? DateOfJoining { get; set; }
        public string? DateOfBirth { get; set; }
        public string? BranchName { get; set; }
        public string? GradeName { get; set; }
        public string? DesignationName { get; set; }
        public string? DepartmentName { get; set; }
        public string? LoginAlias { get; set; }

    }
}
