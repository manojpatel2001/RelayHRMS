using HRMS_Core.DbContext;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.EmployeeMaster
{
    [Table("Employee")]
    public class Employee:BaseModel
    {
        [Key]
        public int EmployeeId { get; set; }
        public string? Initial { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? EmployeeCode { get; set; }
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
    }

}
