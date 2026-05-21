using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class ManpowerRequisitionEmailDetailsModel
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public string ToEmails { get; set; }

        public string CcEmails { get; set; }
        public string BccEmails { get; set; }
        public string Subject { get; set; }
        public string TemplateName { get; set; }
        public string HRContactNumber { get; set; }
        public string HRContactEmail { get; set; }
        public TimeSpan EmailSendTime { get; set; }
        public bool IsActive { get; set; }

        // Extra info
        public int ManpowerRequisitionId { get; set; }
        public string ReportingToId { get; set; }
        public string ToCreatedByEmails { get; set; }
        public string ToCreatedByFullname { get; set; }
    }
}
