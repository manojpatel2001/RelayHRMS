using System;

namespace HRMS_Core.VM.Ess.RecentActivity
{
    public class vmRecentActivityItem
    {
        public string ActivityType { get; set; }
        public DateTime ActivityDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public string? RelatedEmployeeName { get; set; }
        public int SourceId { get; set; }
    }
}
