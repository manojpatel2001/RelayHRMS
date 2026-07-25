using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class AccessRequestViewModel
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public string Subject { get; set; }
        public string TemplateName { get; set; }
        public string HRContactNumber { get; set; }
        public string HRContactEmail { get; set; }
        public TimeSpan EmailSendTime { get; set; }
        public bool IsActive { get; set; }
        public int ManpowerRequisitionId { get; set; }
        public int ReportingToId { get; set; }
        public bool SystemRequire { get; set; }
        public bool EmailIdRequire { get; set; }
        public bool SIMRequire { get; set; }
        public bool ERP_ID { get; set; }
        public string ToEmail_ReportingTo { get; set; }
        public string ToFullname_ReportingTo { get; set; }
        public string ToEmail_CreatedBy { get; set; }
        public string ToFullname_CreatedBy { get; set; }
        public string CcEmail_ZoneHR { get; set; }
        public string ToEmail_ITDepartment { get; set; }
        public string ToEmail_HRDepartment { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int ZoneId { get; set; }
    }
}
