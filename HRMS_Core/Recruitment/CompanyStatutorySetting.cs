using System;

namespace HRMS_Core.Recruitment
{
    public class CompanyStatutorySetting
    {
        public int CompanyStatutorySettingId { get; set; }
        public int CompanyId { get; set; }

        public bool IsPFEnabled { get; set; } = true;
        public decimal PFPercentage { get; set; } = 12.00m;
        public decimal PFCapAmount { get; set; } = 1800.00m;

        public bool IsESIEnabled { get; set; } = true;
        public decimal ESIPercentage { get; set; } = 0.75m;
        public decimal EmployerESIPercentage { get; set; } = 3.25m;
        public decimal ESIGrossCeiling { get; set; } = 21000.00m;

        public bool IsProfessionalTaxEnabled { get; set; } = true;
        public decimal ProfessionalTaxThreshold { get; set; } = 12000.00m;
        public decimal ProfessionalTaxAmount { get; set; } = 200.00m;

        public bool IsGroupMedicalEnabled { get; set; } = true;
        public decimal GroupMedicalGrossThreshold { get; set; } = 21000.00m;
        public decimal GroupMedicalAmount { get; set; } = 266.00m;

        public bool IsTermInsuranceEnabled { get; set; } = true;

        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        // Joined display field
        public string? CompanyName { get; set; }
    }
}
