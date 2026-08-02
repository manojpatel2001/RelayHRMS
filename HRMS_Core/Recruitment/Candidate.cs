using System;

namespace HRMS_Core.Recruitment
{
    public class Candidate
    {
        public int CandidateId { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? AlternatePhone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? CurrentAddress { get; set; }
        public string? CurrentCity { get; set; }
        public string? PreferredLocation { get; set; }
        public string? CurrentEmployer { get; set; }
        public string? CurrentDesignation { get; set; }
        public decimal? TotalExperienceYears { get; set; }
        public decimal? RelevantExperienceYears { get; set; }
        public decimal? CurrentCTC { get; set; }
        public decimal? ExpectedCTC { get; set; }
        public int? NoticePeriodDays { get; set; }
        public string? LinkedInUrl { get; set; }
        public string? GitHubUrl { get; set; }
        public string? PortfolioUrl { get; set; }
        public string? ResumeUrl { get; set; }
        public string? ResumeFileHash { get; set; }
        public string? Source { get; set; }
        public int? ReferredByEmployeeId { get; set; }
        public string? Summary { get; set; }
        public bool IsBlacklisted { get; set; } = false;
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
