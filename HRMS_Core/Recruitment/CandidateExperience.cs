using System;

namespace HRMS_Core.Recruitment
{
    public class CandidateExperience
    {
        public int CandidateExperienceId { get; set; }
        public int CandidateId { get; set; }
        public string? CompanyName { get; set; }
        public string? Designation { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsCurrentEmployer { get; set; } = false;
        public int? DurationMonths { get; set; }
        public string? Responsibilities { get; set; }
        public string? Location { get; set; }
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
