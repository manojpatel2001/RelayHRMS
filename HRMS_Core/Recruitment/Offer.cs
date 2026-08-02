using System;

namespace HRMS_Core.Recruitment
{
    public class Offer
    {
        public int OfferId { get; set; }
        public int CandidateApplicationId { get; set; }
        public int JobPositionId { get; set; }
        public int? OfferSalaryFitmentId { get; set; }
        public decimal? OfferedCTC { get; set; }
        public int? DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public int? BranchId { get; set; }
        public int? GradeId { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? OfferExpiryDate { get; set; }
        public string? OfferStatus { get; set; } = "Draft";
        public string? OfferLetterUrl { get; set; }
        public DateTime? ReleasedDate { get; set; }
        public DateTime? AcceptedDate { get; set; }
        public string? AcceptedByCandidateName { get; set; }
        public string? DeclineReason { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Joined display fields
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? PositionTitle { get; set; }
        public decimal? MinBudget { get; set; }
        public decimal? MaxBudget { get; set; }
        public string? DesignationName { get; set; }
        public string? DepartmentName { get; set; }
        public string? BranchName { get; set; }
        public string? GradeName { get; set; }
    }
}
