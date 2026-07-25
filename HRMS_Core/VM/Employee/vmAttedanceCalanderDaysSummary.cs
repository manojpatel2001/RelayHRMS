namespace HRMS_Core.VM.Employee
{
    public class vmAttedanceCalanderDaysSummary
    {
        public int? TotalPresent { get; set; }
        public int? TotalAbsent { get; set; }
        public int? TotalWeekends { get; set; }
        public int? TotalHolidays { get; set; }
        public int? TotalLeaves { get; set; }
        public int? AttendancePercentage { get; set; }
    }

}
