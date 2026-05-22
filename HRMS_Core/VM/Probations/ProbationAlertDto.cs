using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Probations
{
    public class ProbationAlertDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string EmployeeEmail { get; set; }
        public string GradeName { get; set; }
        public string GradeCategory { get; set; }
        public DateTime ProbationEndDate { get; set; }
        public DateTime NotificationStartDate { get; set; }
        public DateTime EscalationDate { get; set; }
        public DateTime HRDAlertDate { get; set; }
        public DateTime TodayDate { get; set; }

        // ✅ NOTIFY_MANAGER / ESCALATED_TO_HR / HRD_ALERT
        public string AlertStatus { get; set; }
        public int? ReceiverUserId { get; set; }
        public string EmailSubject { get; set; }
        public string NotificationTarget { get; set; }
        public int DaysRemainingToConfirmation { get; set; }
        public int ReportingManagerId { get; set; }
        public int DepartmentId { get; set; }
        public int CompanyId { get; set; }

    }

    public class ManpowerEscalationData
    {
        public int ManpowerRequisitionId { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string PreviousApproverEmail { get; set; }
        public string PreviousApprover { get; set; }
        public string PreviousApproverCode { get; set; }
        public int PreviousLevelNo { get; set; }
        public string NextApproverEmail { get; set; }
        public string NextApprover { get; set; }
        public string NextApproverCode { get; set; }
        public int NextLevelNo { get; set; }
        public DateTime EscalationDate { get; set; }
    }
}
