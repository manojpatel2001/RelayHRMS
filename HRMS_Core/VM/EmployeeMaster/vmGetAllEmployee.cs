using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.EmployeeMaster
{
    public class vmGetAllEmployee
    {
        public int? Id { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? AlfaEmployeeCode { get; set; }
        public string? AlfaCode { get; set; }
        public DateTime? DateOfJoining { get; set; }

        public int? BranchId { get; set; }
        public string? BranchName { get; set; }

        public int? GradeId { get; set; }
        public string? GradeName { get; set; }


        public int? DesignationId { get; set; }
        public string? DesignationName { get; set; }


        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }


        public string? LoginAlias { get; set; }

    }
}
