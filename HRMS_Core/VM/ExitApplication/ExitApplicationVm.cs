using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.ExitApplication
{
    public class ExitApplicationVm
    {
        public int ExitApplicationID { get; set; }

        public DateTime ResignationDate { get; set; }

        public int EmployeeId { get; set; }

        public int NoticePeriodDays { get; set; } = 0;

        public DateTime LastWorkingDate { get; set; }

        public int ShortFallDays { get; set; } = 0;

        public string ReasonForResignation { get; set; }

        public string Comments { get; set; }

        public string DocumentName { get; set; }

        public byte[] DocumentData { get; set; }

        public bool IsAgreementAccepted { get; set; } = false;

        // Status fields
        public bool IsPending { get; set; } = true;
        public bool IsApproved { get; set; } = false;
        public bool IsRejected { get; set; } = false;
        public bool IsApprovedBYHR { get; set; } = false;
        public bool IsNOCFormFilled { get; set; } = false;

        // Audit fields
        public bool IsDeleted { get; set; } = false;
        public bool IsEnabled { get; set; } = true;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
    }
}
