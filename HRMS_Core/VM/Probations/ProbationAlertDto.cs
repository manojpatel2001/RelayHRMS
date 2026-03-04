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
}
