namespace HRMS_Core.VM.UpdateEmployee
{
    public class UpdateBasicInfoViewModel
    {
        public int? Id { get; set; }
        public string? Initial { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? AlfaCode { get; set; }
        public string? AlfaEmployeeCode { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public int? BranchId { get; set; }
        public int? GradeId { get; set; }
        public string? ShiftMasterId { get; set; }
        public string? CTC { get; set; }
        public int? DesignationId { get; set; }
        public decimal? GrossSalary { get; set; }
        public string? CategoryId { get; set; }
        public decimal? BasicSalary { get; set; }
        public int? DepartmentId { get; set; }
        public string? EmployeeTypeId { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? LoginAlias { get; set; }
        public int? ReportingManagerId { get; set; }
        public string? SubBranch { get; set; }
        public string? EnrollNo { get; set; }
        public int? CompanyId { get; set; }
        public bool? Overtime { get; set; }
        public bool? Latemark { get; set; }
        public bool? Earlymark { get; set; }
        public bool? Fullpf { get; set; }
        public bool? Pt { get; set; }
        public bool? Fixsalary { get; set; }
        public bool? Probation { get; set; }
        public bool? Trainee { get; set; }
        public bool? IsPFApplicable { get; set; }
        public int? WeekOffDetailsId { get; set; }
        public bool? IsPermissionPunchInOut { get; set; }
        public decimal? ProbationCompletionPeriod { get; set; }
        public string? ProbationPeriodType { get; set; }
        public int? RoleId { get; set; }
        public string? UpdatedBy { get; set; }
    }

}
