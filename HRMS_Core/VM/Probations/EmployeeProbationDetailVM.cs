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
        public string? PersonalEmailId { get; set; }
        public string? CompanyName { get; set; }
        public int? CompanyId { get; set; }
    }

    public class ApproverDetailsViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeEmailId { get; set; }
        public string CompanyName { get; set; }

        // Previous Approval Level Details
        public string PreviousApproverName { get; set; }
        public string PreviousApproverCode { get; set; }
        public int PreviousApproverId { get; set; }
        public string PreviousApproverEmailId { get; set; }
        public int PreviousSequenceNo { get; set; }

        // Next Approval Level Details
        public string NextApproverName { get; set; }
        public string NextApproverCode { get; set; }
        public int NextApproverId { get; set; }
        public string NextApproverEmailId { get; set; }
        public int NextSequenceNo { get; set; }
    }
   

}
