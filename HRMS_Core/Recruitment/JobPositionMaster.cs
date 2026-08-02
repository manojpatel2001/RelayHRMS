using System;

namespace HRMS_Core.Recruitment
{
    public class JobPositionMaster
    {
        public int JobPositionId { get; set; }
        public int ManpowerRequisitionId { get; set; }
        public string? PositionTitle { get; set; }
        public int? DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public int? BranchId { get; set; }
        public int? CompanyId { get; set; }
        public int? GradeId { get; set; }
        public string? EmploymentType { get; set; }
        public int VacancyCount { get; set; } = 1;
        public int FilledCount { get; set; } = 0;
        public decimal? MinExperienceYears { get; set; }
        public decimal? MaxExperienceYears { get; set; }
        public string? MinEducationLevel { get; set; }
        public string? RequiredSkills { get; set; }
        public string? PreferredSkills { get; set; }
        public string? JobDescription { get; set; }
        public decimal? MinBudget { get; set; }
        public decimal? MaxBudget { get; set; }
        public string? Priority { get; set; }
        public int? OwnerRecruiterId { get; set; }
        public int? ReportingManagerId { get; set; }
        public string? Status { get; set; } = "Open";
        public DateTime? TargetClosureDate { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        // Joined display fields (read-shape only)
        public string? DesignationName { get; set; }
        public string? DepartmentName { get; set; }
        public string? BranchName { get; set; }
        public string? GradeName { get; set; }
    }
}
