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

}
