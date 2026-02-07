using System;
using System.Collections.Generic;

namespace HRMS_Core.VM.EmailService
{
    

    public class EmailReport
    {
        public int ReportId { get; set; }
        public string? ReportName { get; set; }
        public string? ToEmails { get; set; }
        public string? CcEmails { get; set; }
        public string? BccEmails { get; set; }
        public string? Subject { get; set; }
        public string? TemplateName { get; set; }
        public bool? IsActive { get; set; }
        public string? HRContactNumber { get; set; }
        public string? HRContactEmail { get; set; }
        public TimeSpan? EmailSendTime { get; set; }
    }

    public class DailyAbsentReportResult
    {
        public int ReportingManagerId { get; set; }
        public string? ReportingManagerName { get; set; }
        public string? ReportingManagerEmail { get; set; }
        public DateTime? AttendanceDate { get; set; }


        // Initialize list to avoid null reference
        public List<EmployeeRecord> Employees { get; set; } = new();
    }

    public class EmployeeRecord
    {
        public int EmployeeRecordId { get; set; }
        public int EmployeeId { get; set; }
        public int BranchId { get; set; }

        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; }
        public string? BranchName { get; set; }
        public string? Attendance { get; set; }

    }

    public class TodayLeftEmployeeEmailVM
    {
        public int? SerialNo { get; set; }
        public string? BranchName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? Name { get; set; }
        public string? LeftDate { get; set; }
        public string? LeftEnteredOn { get; set; }
    }

}
