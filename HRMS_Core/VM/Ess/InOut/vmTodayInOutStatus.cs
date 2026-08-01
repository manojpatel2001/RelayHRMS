using System;

namespace HRMS_Core.VM.Ess.InOut
{
    public class vmTodayInOutStatus
    {
        public int PunchCount { get; set; }
        public DateTime? LastClockIn { get; set; }
        public DateTime? LastClockOut { get; set; }
        public bool IsClockedIn { get; set; }
        public long WorkedSeconds { get; set; }
    }
}
