using HRMS_Core.Master.JobMaster;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.EmployeeMaster
{
    public class vmEmployeeData
    {
        public int? Id { get; set; }
        public string? Initial { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? AlfaEmployeeCode { get; set; }
        public string? AlfaCode { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public int? BranchId { get; set; }
        
        public int? GradeId { get; set; }
        
        public int? ShiftMasterId { get; set; }

        public string? CTC { get; set; }
        public int? DesignationId { get; set; }
        
        public decimal? GrossSalary { get; set; }

        public int? CategoryId { get; set; }
        public decimal? BasicSalary { get; set; }
        public int? DepartmentId { get; set; }

        public int? EmployeeTypeId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int? UserPrivilege { get; set; }
        public int? RoleId { get; set; }

        public string? LoginAlias { get; set; }
        public string? Password { get; set; }
        public int? ReportingManagerId { get; set; }
        public string? SubBranch { get; set; }
        public string? EnrollNo { get; set; }

        public int? CompanyId { get; set; }
        
        public bool? Overtime { get; set; } = false;
        public bool? Latemark { get; set; } = false;
        public bool? Earlymark { get; set; } = false;
        public bool? Fullpf { get; set; } = false;
        public bool? Pt { get; set; } = false;
        public bool? Fixsalary { get; set; } = false;
        public bool? Probation { get; set; } = false;
        public bool? Trainee { get; set; } = false;
        public int? WeekOffDetailsId { get; set; }
        public bool? IsPermissionPunchInOut { get; set; } = false;
        public bool? IsLeft { get; set; } = false;
        //base model
        public bool? IsDeleted { get; set; } = false;
        public bool? IsEnabled { get; set; } = true;
        public bool? IsBlocked { get; set; } = false;
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }

        public string? EmployeeProfileUrl { get; set; }
        public string? EmployeeSignatureUrl { get; set; }
    }
}
