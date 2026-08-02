using System;

namespace HRMS_Core.Recruitment
{
    public class CandidatePipelineStage
    {
        public int CandidatePipelineStageId { get; set; }
        public string? StageName { get; set; }
        public string? StageCategory { get; set; }
        public int SortOrder { get; set; }
        public bool IsTerminal { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public int? CompanyId { get; set; }
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
