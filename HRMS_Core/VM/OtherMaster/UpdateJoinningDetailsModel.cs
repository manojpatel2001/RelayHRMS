namespace HRMS_Core.VM.OtherMaster
{
    public class UpdateJoinningDetailsModel
    {
        public string? Action { get; set; }
        public int? ManpowerRequisitionId { get; set; }

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
        public decimal? GrossSalary { get; set; }
        public decimal? NetSalary { get; set; }

        // Contact Details Tab Fields
        public string? ContactNo1 { get; set; }
        public string? ContactPersonName1 { get; set; }
        public string? ContactPersonRelation1 { get; set; }
        public string? ContactNo2 { get; set; }
        public string? ContactPersonName2 { get; set; }
        public string? ContactPersonRelation2 { get; set; }
        public string? ContactNo3 { get; set; }
        public string? ContactPersonName3 { get; set; }
        public string? ContactPersonRelation3 { get; set; }

        // Document Details Tab Fields
        public string? AadharCardNo { get; set; }
        public string? AadharCardCopyPath { get; set; }
        public string? PanCardNo { get; set; }
        public string? PanCardCopyPath { get; set; }
        public string? FreshResumePath { get; set; }
        public string? PassportSizePhotoCopyPath { get; set; }
        public string? CancelledChequePath { get; set; }
        public string? PayslipPath { get; set; }
        public string? ExperienceCertificatePath { get; set; }
    }


    public class GetAllJoiningManpowerRequisitions
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
        public bool? IsEnabled { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? SerialNo { get; set; }
        public int? CompanyId { get; set; }
        public string? RM_Approved { get; set; }
        public string? NM_Approved { get; set; }
        public string? HR_Approved { get; set; }
        public int? BranchId { get; set; }
        public string? OperationType { get; set; }
        public string? PresentAddress { get; set; }
        public string? PermanentAddress { get; set; }
        public string? MaritalStatus { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? BloodGroup { get; set; }
        public string? InterviewedBy { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string? InterviewPlace { get; set; }
        public string? ClientName { get; set; }
        public string? ERPCode { get; set; }
        public string? PreviousCompanyUAN { get; set; }
        public string? PreviousCompanyESIC { get; set; }
        public string? PFUAN { get; set; }
        public string? ESICNo { get; set; }
        public string? PAN { get; set; }

        // Joined fields
        public string? DesignationName { get; set; }
        public string? DepartmentName { get; set; }
        public string? ReportingCode { get; set; }
        public string? ReportingName { get; set; }
        public string? RequestedBy { get; set; }
        public string? BranchName { get; set; }
        public string? RequestedDesignationName { get; set; }
    }

}
