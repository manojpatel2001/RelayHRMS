using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.EmployeeMaster
{
    public class vmEmployeeData
    {
        public string? Initial { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? EmployeeStringCode { get; set; }
        public string? EmployeeNumberCode { get; set; }
        public DateTime? DateOfJoining { get; set; }

        public string? Branch { get; set; }
        public string? Grade { get; set; }
        public string? Shift { get; set; }

        public string? CTC { get; set; }
        public string? Designation { get; set; }
        public decimal? GrossSalary { get; set; }

        public string? Category { get; set; }
        public decimal? BasicSalary { get; set; }
        public string? Department { get; set; }

        public string? EmployeeType { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? UserPrivilege { get; set; }

        public string? LoginAlias { get; set; }

        public bool? Overtime { get; set; } = false;
        public bool? Latemark { get; set; } = false;
        public bool? Earlymark { get; set; } = false;
        public bool? Fullpf { get; set; } = false;
        public bool? Pt { get; set; } = false;
        public bool? Fixsalary { get; set; } = false;
        public bool? Probation { get; set; } = false;
        public bool? Trainee { get; set; } = false;
    }
}
