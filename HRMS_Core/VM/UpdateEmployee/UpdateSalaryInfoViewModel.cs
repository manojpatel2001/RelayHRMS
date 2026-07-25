namespace HRMS_Core.VM.UpdateEmployee
{
    public class UpdateSalaryInfoViewModel
    {
        public int? Id { get; set; }
        public string? PrimaryPaymentMode { get; set; }
        public string? PrimaryBankName { get; set; }
        public string? PrimaryIFSCCode { get; set; }
        public string? PrimaryAccountNumber { get; set; }
        public string? PrimaryBankBranchName { get; set; }
        public string? WagesTypes { get; set; }
        public DateTime? GroupJoiningDate { get; set; }
        public int? BusinessSegmentId { get; set; }
        public string? EmployeeSalaryReport { get; set; }
        public string? EmployeePFReport { get; set; }
        public string? EmployeePTReport { get; set; }
        public string? EmployeeTaxReport { get; set; }
        public string? EmployeeESIReport { get; set; }
        public string? EmployeeNamePrmaryBank { get; set; }
        public string? UpdatedBy { get; set; }
    }

}
