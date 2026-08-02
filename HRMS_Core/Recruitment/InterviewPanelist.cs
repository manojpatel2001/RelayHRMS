using System;

namespace HRMS_Core.Recruitment
{
    public class InterviewPanelist
    {
        public int InterviewPanelistId { get; set; }
        public int InterviewId { get; set; }
        public int EmployeeId { get; set; }
        public bool IsPrimary { get; set; } = false;
        public string? InviteStatus { get; set; } = "Pending";
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
