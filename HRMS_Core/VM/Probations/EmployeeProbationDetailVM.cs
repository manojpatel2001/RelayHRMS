namespace HRMS_Core.VM.Probations
{
    public class EmployeeProbationDetailVM
    {
        public int? EmployeeId { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public DateTime? ProbationEndDate { get; set; }
        public string? BranchName { get; set; }
        public int? BranchId { get; set; }
        public string? DepartmentName { get; set; }
        public int? DepartmentId { get; set; }
        public string? DesignationName { get; set; }
        public int? DesignationId { get; set; }
    }
}
