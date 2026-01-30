namespace HRMS_Core.VM.EmployeeMaster
{
    public class vmEmployeeIncrementSalary
    {
        public int? Id { get; set; }
        public int? EmployeeId { get; set; }
        public int? CompanyId { get; set; }
        public decimal? GrossSalary { get; set; }
        public decimal? BasicSalary { get; set; }
        public bool? IsPFApplicable { get; set; }
        public int? ReasonId { get; set; }
        public DateTime? EffectedDate { get; set; }
    }

}
