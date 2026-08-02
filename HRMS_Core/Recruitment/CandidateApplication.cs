using System;

namespace HRMS_Core.Recruitment
{
    public class CandidateApplication
    {
        public int CandidateApplicationId { get; set; }
        public int CandidateId { get; set; }
        public int JobPositionId { get; set; }
        public int? JobPostingId { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public string? ApplicationSource { get; set; }
        public string? ResumeUrl { get; set; }
        public string? CoverLetter { get; set; }
        public int CurrentPipelineStageId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        // Joined display fields (read-shape only)
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? CurrentEmployer { get; set; }
        public decimal? TotalExperienceYears { get; set; }
        public decimal? ExpectedCTC { get; set; }
        public string? PositionTitle { get; set; }
        public string? StageName { get; set; }
        public string? StageCategory { get; set; }
        public decimal? OverallScore { get; set; }
        public string? Recommendation { get; set; }
    }
}
