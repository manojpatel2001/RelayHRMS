using System;

namespace HRMS_Core.Recruitment
{
    public class SelectionDecision
    {
        public int SelectionDecisionId { get; set; }
        public int CandidateApplicationId { get; set; }
        public string? DecisionStatus { get; set; }
        public int? DecidedBy { get; set; }
        public DateTime? DecisionDate { get; set; }
        public string? Remarks { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
