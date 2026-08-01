using System;

namespace HRMS_Core.VM.Ess.RecentActivity
{
    public class vmEmployeeLifecycleEvent
    {
        public string ActivityType { get; set; }
        public DateTime ActivityDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public int SourceId { get; set; }
    }
}
