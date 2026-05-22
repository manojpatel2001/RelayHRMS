namespace HRMS_Core.VM.Employee
{
    public class vmAttedanceCalanderDays
    {
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? BranchName { get; set; }
        public DateTime? CalendarDate { get; set; }
        public int? DayNumber { get; set; }
        public string? DayName { get; set; }
        public bool? IsWeekend { get; set; }
        public string? AttendanceStatus { get; set; }
        public decimal? WorkingHours { get; set; }
        public string? InTime { get; set; }
        public string? OutTime { get; set; }
        public decimal? SalaryDay { get; set; }
        public string? HolidayName { get; set; }
        public string? Holidaycategory { get; set; }
        public string? LeaveTypeName { get; set; }
        public string? LeaveReason { get; set; }
        public DateTime? LeaveFromDate { get; set; }
        public DateTime? LeaveToDate { get; set; }
        public string? StatusDescription { get; set; }
        public string? StatusColor { get; set; }
    }

}
