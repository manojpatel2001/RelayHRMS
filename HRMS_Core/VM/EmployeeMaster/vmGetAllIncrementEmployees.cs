namespace HRMS_Core.VM.EmployeeMaster
{
    public class vmGetAllIncrementEmployees
    {
        public int? EmployeeId { get; set; }
        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? BranchName { get; set; }
        public decimal? OldBasicSalary { get; set; }
        public decimal? NewBasicSalary { get; set; }
        public decimal? IncrementedAmount { get; set; }
        public DateTime? EffectedDate { get; set; }
        public string? Reason { get; set; }
    }

}
