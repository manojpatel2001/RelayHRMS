using System;

namespace HRMS_Core.Recruitment
{
    public class CandidateOnboardingChecklist
    {
        public int CandidateOnboardingChecklistId { get; set; }
        public int CandidateApplicationId { get; set; }
        public bool OfferAcceptedConfirmed { get; set; }
        public bool DocumentsComplete { get; set; }
        public bool BGVComplete { get; set; }
        public bool MedicalComplete { get; set; }
        public bool SalaryApproved { get; set; }
        public bool JoiningApproved { get; set; }
        public bool EmployeeCreationApproved { get; set; }
        public string? FinalRemarks { get; set; }
        public int? FinalApprovedBy { get; set; }
        public DateTime? FinalApprovedDate { get; set; }
        public string? JoiningStatus { get; set; } = "Pending";
        public DateTime? ActualJoiningDate { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
