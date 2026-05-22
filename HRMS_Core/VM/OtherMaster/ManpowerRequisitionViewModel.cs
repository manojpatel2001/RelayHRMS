namespace HRMS_Core.VM.OtherMaster
{
    public class ManpowerRequisitionViewModel
    {
        public int? ManpowerRequisitionId { get; set; }
        public int? DepartmentId { get; set; }
        public string? RequirementType { get; set; }
        public string? EmployeeName { get; set; }
        public string? PersonalEmail { get; set; }
        public string? ContactNumber { get; set; }
        public string? ClosureBy { get; set; }
        public int? DesignationId { get; set; }
        public string? ExperienceRange { get; set; }
        public string? EducationalQualification { get; set; }
        public string? ComputerSkills { get; set; }
        public string? JobResponsibility { get; set; }
        public int? Age { get; set; }
        public string? Gender { get; set; }
        public string? OtherBenefits { get; set; }
        public string? SystemRequire { get; set; }
        public string? EmailIdRequire { get; set; }
        public string? SIMRequire { get; set; }
        public string? MobileHandsetRequire { get; set; }
        public int? ReportingToId { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string? CategoryOfEmployment { get; set; }
        public decimal? CTC_Monthly { get; set; }
        public decimal? GrossSalary { get; set; }
        public decimal? TakeHomeSalary { get; set; }

        public DateTime? CreatedDate { get; set; }

        public string? DesignationName { get; set; }
        public string? DepartmentName { get; set; }
        public string? SerialNo { get; set; }
        public string? ReportingCode { get; set; }
        public string? ReportingName { get; set; }
        public string? RequestedBy { get; set; }
        public string? HR_Approved { get; set; }
        public string? NM_Approved { get; set; }
        public string? RM_Approved { get; set; }
        public string? BranchName { get; set; }

        // Employee Info Tab Fields
        public string? OperationType { get; set; }
       
        public string? PresentAddress { get; set; }
        public string? PermanentAddress { get; set; }
        public string? MaritalStatus { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? BloodGroup { get; set; }

        // Joining Details Tab Fields
        public string? InterviewedBy { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string? InterviewPlace { get; set; }
        public string? ClientName { get; set; }
        public string? ERPCode { get; set; }
        public string? PreviousCompanyUAN { get; set; }
        public string? PreviousCompanyESIC { get; set; }

        // Salary Details Tab Fields
        public string? PFUAN { get; set; }
        public string? ESICNo { get; set; }
        public string? PAN { get; set; }

        
    }
   
}
