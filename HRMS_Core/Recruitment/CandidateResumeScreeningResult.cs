using System;

namespace HRMS_Core.Recruitment
{
    public class CandidateResumeScreeningResult
    {
        public int CandidateResumeScreeningResultId { get; set; }
        public int CandidateApplicationId { get; set; }
        public int JobPositionId { get; set; }
        public decimal? SkillMatchScore { get; set; }
        public decimal? ExperienceMatchScore { get; set; }
        public decimal? EducationMatchScore { get; set; }
        public decimal OverallScore { get; set; }
        public string? Recommendation { get; set; }
        public string? MatchedSkills { get; set; }
        public string? MissingSkills { get; set; }
        public bool IsDuplicate { get; set; } = false;
        public int? DuplicateOfCandidateId { get; set; }
        public string? DuplicateMatchReason { get; set; }
        public DateTime? ScreenedDate { get; set; }
        public string? ScreeningEngineVersion { get; set; } = "RuleBased-v1";
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
