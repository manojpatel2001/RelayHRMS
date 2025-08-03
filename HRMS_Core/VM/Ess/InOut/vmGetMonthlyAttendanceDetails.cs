namespace HRMS_Core.VM.Ess.InOut
{
    public class vmGetMonthlyAttendanceDetails
    {
        public int? EmployeeId { get; set; }
        public string? InTime { get; set; }
        public string? OutTime { get; set; }
        public decimal? WorkingHours { get; set; }
        public string? AttendanceStatus { get; set; }
        public DateTime? ShiftDate { get; set; }
        public decimal? SalaryDay { get; set; } 
        public string? FullName { get; set; }
        public string? BranchName { get; set; }
        public string? EmployeeCode { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
    }
}
