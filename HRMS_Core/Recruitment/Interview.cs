using System;

namespace HRMS_Core.Recruitment
{
    public class Interview
    {
        public int InterviewId { get; set; }
        public int CandidateApplicationId { get; set; }
        public string? RoundName { get; set; }
        public int RoundSequence { get; set; } = 1;
        public DateTime ScheduledDate { get; set; }
        public int? DurationMinutes { get; set; }
        public string? Mode { get; set; }
        public string? MeetingLink { get; set; }
        public string? Location { get; set; }
        public string? Status { get; set; } = "Scheduled";
        public string? Remarks { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        // Joined display fields
        public int? CandidateId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? PositionTitle { get; set; }
    }
}
