namespace HRMS_Core.VM.EmployeeMaster
{
    public class salaryPara
    {
        public decimal? GrossSalary { get; set; }
        public decimal? BasicSalary { get; set; }
        public int? CompanyId { get; set; }
        public int? EmployeeId { get; set; }
        public bool? IsPFApplicable { get; set; } = true;
    }
}
