using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.OtherMaster
{
    public class vmOrganizationPolicy
    {
        public int OrganizationPolicyId { get; set; }
        public string? OrganizationPolicyName { get; set; }
        public string? ToolTip { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? Sorting { get; set; }
        public string? DocumentUrl { get; set; }
        public string? Grouping { get; set; }
        public string? GroupingValue { get; set; }
        public IFormFile? DocumentFile { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public bool? IsEnabled { get; set; } = true;
        public bool? IsBlocked { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? DeletedBy { get; set; }
    }
}
