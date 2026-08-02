using System;

namespace HRMS_Core.Recruitment
{
    public class CandidateProject
    {
        public int CandidateProjectId { get; set; }
        public int CandidateId { get; set; }
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
        public string? RoleInProject { get; set; }
        public string? TechnologiesUsed { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ProjectUrl { get; set; }
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
