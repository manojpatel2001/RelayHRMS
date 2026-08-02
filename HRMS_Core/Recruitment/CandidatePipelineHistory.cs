using System;

namespace HRMS_Core.Recruitment
{
    public class CandidatePipelineHistory
    {
        public int CandidatePipelineHistoryId { get; set; }
        public int CandidateApplicationId { get; set; }
        public int? FromStageId { get; set; }
        public int ToStageId { get; set; }
        public DateTime? TransitionDate { get; set; }
        public string? Remarks { get; set; }
        public int? ActionBy { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        // Joined display fields (read-shape only)
        public string? FromStageName { get; set; }
        public string? ToStageName { get; set; }
    }
}
