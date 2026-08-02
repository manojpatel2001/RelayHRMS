using System;

namespace HRMS_Core.Recruitment
{
    public class InterviewFeedback
    {
        public int InterviewFeedbackId { get; set; }
        public int InterviewId { get; set; }
        public int PanelistEmployeeId { get; set; }
        public int? TechnicalRating { get; set; }
        public int? CommunicationRating { get; set; }
        public int? ProblemSolvingRating { get; set; }
        public int? OverallRating { get; set; }
        public string? Strengths { get; set; }
        public string? Weaknesses { get; set; }
        public string? Recommendation { get; set; }
        public string? Remarks { get; set; }
        public bool IsSubmitted { get; set; } = false;
        public DateTime? SubmittedDate { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Joined display fields
        public string? RoundName { get; set; }
        public DateTime? ScheduledDate { get; set; }
    }
}
