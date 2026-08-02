using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.OtherMaster
{
    public class ManpowerRequisition
    {
        public int? ManpowerRequisitionId { get; set; }
        public int? DepartmentId { get; set; }
        public string? RequirementType { get; set; }
        public string? EmployeeName { get; set; }
        public string? PersonalEmail { get; set; }
        public string? ContactNumber { get; set; }
        public string? ClosureBy { get; set; }
        public int? DesignationId { get; set; }
        public string? ExperienceRange { get; set; }
        public string? EducationalQualification { get; set; }
        public string? ComputerSkills { get; set; }
        public string? JobResponsibility { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? OtherBenefits { get; set; }
        public string? SystemRequire { get; set; }
        public string? EmailIdRequire { get; set; }
        public string? Simrequire { get; set; }
        public string? ErpId { get; set; }
        public int? ReportingToId { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string? CategoryOfEmployment { get; set; }
        //public decimal? CTC_Monthly { get; set; }
        //public decimal? GrossSalary { get; set; }
        public decimal? TakeHomeSalary { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int? CompanyId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public decimal? Amount { get; set; }
        public int? NumberOfPosition { get; set; }
        public int? JobCategory { get; set; }
        public int? BranchId { get; set; }           // ✅ NEW
        public string? CustomerName { get; set; }

        // Phase 1 Recruitment overhaul — additive fields (see 2026-08-02_ManpowerRequisition_Phase1Extension.sql).
        // ReplacementOrNew is intentionally not added: RequirementType already covers New Position/Replacement.
        public string? Priority { get; set; }
        public int? ReplacedEmployeeId { get; set; }
        public string? Remarks { get; set; }
        public decimal? BudgetAmount { get; set; }
    }

}
