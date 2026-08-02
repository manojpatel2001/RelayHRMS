using System;

namespace HRMS_Core.Recruitment
{
    public class OfferApprovalLevelConfig
    {
        public int OfferApprovalLevelConfigId { get; set; }
        public int? CompanyId { get; set; }
        public int LevelNo { get; set; }
        public string? ApproverRole { get; set; }
        public int? FixedApproverEmployeeId { get; set; }
        public int EscalationDays { get; set; } = 2;
        public bool IsActive { get; set; } = true;
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
