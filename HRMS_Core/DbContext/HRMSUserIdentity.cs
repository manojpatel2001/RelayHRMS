using HRMS_Core.ControlPanel.CompanyInformation;
using HRMS_Core.Employee;
using HRMS_Core.Master.JobMaster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.DbContext
{
    public class HRMSUserIdentity : IdentityUser
    {
        
        public string? Initial { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public int? BranchId { get; set; }
        [ForeignKey(nameof(BranchId))]
        [ValidateNever]
        public Branch? Branch { get; set; }

        public int? GradeId { get; set; }
        [ForeignKey(nameof(GradeId))]
        [ValidateNever]
        public Grade? Grade { get; set; }
        public int? ShiftMasterId { get; set; }
        [ForeignKey(nameof(ShiftMasterId))]
        [ValidateNever]
        public ShiftMaster? ShiftMaster  { get; set; }

        public string? CTC { get; set; }
        public int? DesignationId { get; set; }
        [ForeignKey(nameof(DesignationId))]
        [ValidateNever]
        public Designation? Designation{ get; set; }
        public decimal? GrossSalary { get; set; }

        public int? CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        [ValidateNever]
        public Category? Category { get; set; }
        public decimal? BasicSalary { get; set; }
        public int? DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        [ValidateNever]
        public Department? Department { get; set; }

        public int? EmployeeTypeId { get; set; }
        [ForeignKey(nameof(EmployeeTypeId))]
        [ValidateNever]
        public EmployeeType? EmployeeType { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? UserPrivilege { get; set; }

        public string? LoginAlias { get; set; }
        public string? Password { get; set; }
        public string? ReportingManager { get; set; }
        public string? SubBranch { get; set; }
        public string? EnrollNo { get; set; }

        public int? CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [ValidateNever]
        public CompanyDetails? CompanyDetails { get; set; }


        public bool? Overtime { get; set; } = false;
        public bool? Latemark { get; set; } = false;
        public bool? Earlymark { get; set; } = false;
        public bool? Fullpf { get; set; } = false;
        public bool? Pt { get; set; } = false;
        public bool? Fixsalary { get; set; } = false;
        public bool? Probation { get; set; } = false;
        public bool? Trainee { get; set; } = false;

        //base model
        public bool? IsDeleted { get; set; } = false;
        public bool? IsEnabled { get; set; } = true;
        public bool? IsBlocked { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public string? EmployeeProfileUrl { get; set; }
        public string? EmployeeSignatureUrl { get; set; }
    }
}
