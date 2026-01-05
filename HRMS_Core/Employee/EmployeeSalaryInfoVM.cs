namespace HRMS_Core.Employee
{
    public class EmployeeSalaryInfoVM
    {
        public int EmployeeId { get; set; }
        public decimal BasicSalary { get; set; }
        public decimal GrossSalary { get; set; }
    }
    public class InsertEmployeeSalaryHistoryVM
    {
        public int EmployeeId { get; set; }
        public decimal OldGrossSalary { get; set; }
        public decimal NewGrossSalary { get; set; }
        public decimal OldBasicSalary { get; set; }
        public decimal NewBasicSalary { get; set; }
        public DateTime EffectiveFromDate { get; set; }
        public DateTime? EffectiveToDate { get; set; }
        public int? ReasonId { get; set; }
        public bool IsActive { get; set; } = true;
        public int CreatedBy { get; set; }
    }

}
