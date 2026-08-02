using System;

namespace HRMS_Core.Recruitment
{
    public class JobPosting
    {
        public int JobPostingId { get; set; }
        public int JobPositionId { get; set; }
        public string? PostingTitle { get; set; }
        public string? Slug { get; set; }
        public bool IsInternal { get; set; } = true;
        public bool IsExternal { get; set; } = false;
        public string? JobDescriptionHtml { get; set; }
        public string? ResponsibilitiesHtml { get; set; }
        public string? BenefitsHtml { get; set; }
        public string? Location { get; set; }
        public string? WorkMode { get; set; }
        public DateTime? PublishDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Status { get; set; } = "Draft";
        public int ViewCount { get; set; }
        public int ApplicationCount { get; set; }
        public int? CompanyId { get; set; }

        public bool LinkedInEnabled { get; set; }
        public string? LinkedInStatus { get; set; }
        public DateTime? LinkedInPostedDate { get; set; }
        public string? LinkedInPostingUrl { get; set; }

        public bool NaukriEnabled { get; set; }
        public string? NaukriStatus { get; set; }
        public DateTime? NaukriPostedDate { get; set; }
        public string? NaukriPostingUrl { get; set; }

        public bool IndeedEnabled { get; set; }
        public string? IndeedStatus { get; set; }
        public DateTime? IndeedPostedDate { get; set; }
        public string? IndeedPostingUrl { get; set; }

        public bool ReferralEnabled { get; set; }
        public string? ReferralStatus { get; set; }
        public DateTime? ReferralPostedDate { get; set; }
        public string? ReferralPostingUrl { get; set; }

        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }

        public string? PositionTitle { get; set; }
    }
}
