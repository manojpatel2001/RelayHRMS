using System;

namespace HRMS_Core.Recruitment
{
    public class CandidateSkill
    {
        public int CandidateSkillId { get; set; }
        public int CandidateId { get; set; }
        public string? SkillName { get; set; }
        public string? ProficiencyLevel { get; set; }
        public decimal? ExperienceYears { get; set; }
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
