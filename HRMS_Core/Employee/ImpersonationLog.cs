using System;

namespace HRMS_Core.Employee
{
    public class ImpersonationLog
    {
        public int ImpersonationLogID { get; set; }
        public int AdminId { get; set; }
        public string? AdminEmail { get; set; }
        public int TargetEmployeeId { get; set; }
        public string? TargetEmail { get; set; }
        public int? CompanyId { get; set; }
        public string? IPAddress { get; set; }
        public string? BrowserInfo { get; set; }
        public string? Reason { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
