using System;

namespace HRMS_Core.Recruitment
{
    public class OfferSalaryFitment
    {
        public int OfferSalaryFitmentId { get; set; }
        public int CandidateApplicationId { get; set; }
        public int? SalaryStructureTemplateId { get; set; }
        public decimal? CurrentCTC { get; set; }
        public decimal? ExpectedCTC { get; set; }
        public decimal? RecommendedCTC { get; set; }
        public decimal? FinalCTC { get; set; }
        public string? BudgetValidationStatus { get; set; } = "NotValidated";
        public string? Remarks { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
