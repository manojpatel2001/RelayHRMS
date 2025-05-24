using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.EmployeeMaster
{
    public class vmGetAllEmployee
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Initial { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }
        public DateTime? DateOfJoining { get; set; }

        public int? BranchId { get; set; }
        public string? BranchName { get; set; }

        public int? GradeId { get; set; }
        public string? GradeName { get; set; }

        public string? Shift { get; set; }
        public string? CTC { get; set; }

        public int? DesignationId { get; set; }
        public string? DesignationName { get; set; }

        public decimal? GrossSalary { get; set; }
        public string? Category { get; set; }
        public decimal? BasicSalary { get; set; }

        public int? DepartmentId { get; set; }
        public string? DepartmentName { get; set; }

        public string? EmployeeType { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? UserPrivilege { get; set; }

        public string? LoginAlias { get; set; }
        public string? Password { get; set; }
        public string? ReportingManager { get; set; }
        public string? SubBranch { get; set; }
        public string? EnrollNo { get; set; }

        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }

        public bool? Overtime { get; set; } = false;
        public bool? Latemark { get; set; } = false;
        public bool? Earlymark { get; set; } = false;
        public bool? Fullpf { get; set; } = false;
        public bool? Pt { get; set; } = false;
        public bool? Fixsalary { get; set; } = false;
        public bool? Probation { get; set; } = false;
        public bool? Trainee { get; set; } = false;

        // Base model
        public bool? IsDeleted { get; set; } = false;
        public bool? IsEnabled { get; set; } = true;
        public bool? IsBlocked { get; set; } = false;
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        
        public string? EmployeeProfileUrl { get; set; }
        public string? EmployeeSignatureUrl { get; set; }
    }
}
