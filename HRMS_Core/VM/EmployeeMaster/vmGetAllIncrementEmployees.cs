namespace HRMS_Core.VM.EmployeeMaster
{
    public class vmGetAllIncrementEmployees
    {
        public int? EmployeeId { get; set; }
        public int? Id { get; set; }
        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }
        public decimal? OldBasicSalary { get; set; }
        public decimal? OldGrossSalary { get; set; }
        public decimal? NewBasicSalary { get; set; }
        public decimal? NewGrossSalary { get; set; }
        public DateTime? EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }
        public string? Reason { get; set; }
    }

}
