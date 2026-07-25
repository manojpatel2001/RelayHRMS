using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class ManpowerRequisitionEmailViewModel
    {
        public int ReportId { get; set; }
        public string ReportName { get; set; }
        public string Subject { get; set; }
        public string TemplateName { get; set; }
        public string HRContactNumber { get; set; }
        public string HRContactEmail { get; set; }
        public TimeSpan? EmailSendTime { get; set; }
        public bool IsActive { get; set; }

        // Requisition Info
        public int ManpowerRequisitionId { get; set; }
        public int ReportingToId { get; set; }

        // TO 1: Reporting To
        public string ToEmail_ReportingTo { get; set; }
        public string ToFullname_ReportingTo { get; set; }

        // TO 2: Created By
        public string ToEmail_CreatedBy { get; set; }
        public string ToFullname_CreatedBy { get; set; }

        // TO 3: 1st Approver - Zone RM
        public string ToEmail_1stApprover_RM { get; set; }
        public string ToFullname_1stApprover_RM { get; set; }
        public int? RM_EmployeeCode { get; set; }
        public int? RM_Id { get; set; }

        // TO 4: 2nd Approver - Department HOD
        public string ToEmail_2ndApprover_HOD { get; set; }
        public string ToFullname_2ndApprover_HOD { get; set; }
        public int? HOD_EmployeeCode { get; set; }
        public int? HOD_Id { get; set; }

        // BCC: 3rd Approver - Director
        public string BccEmail_Director { get; set; }
        public string BccFullname_Director { get; set; }
        public int? Director_Id { get; set; }

        // Branch & Zone Info
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public int ZoneId { get; set; }
    }
}
