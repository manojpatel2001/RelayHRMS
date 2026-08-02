using System;

namespace HRMS_Core.Recruitment
{
    public class OfferSalaryBreakupComponent
    {
        public int OfferSalaryBreakupComponentId { get; set; }
        public int OfferSalaryFitmentId { get; set; }
        public string? ComponentName { get; set; }
        public string? ComponentCategory { get; set; }
        public string? Frequency { get; set; } = "Monthly";
        public decimal Amount { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
