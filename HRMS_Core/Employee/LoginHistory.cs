using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Employee
{
    public class LoginHistory
    {
        public int LoginHistoryID { get; set; }
        public int? EmpID { get; set; }
        public DateTime LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public string? IPAddress { get; set; }
        public string? BrowserInfo { get; set; }
        public string? DeviceType { get; set; }
        public bool LoginStatus { get; set; }
        public string? FailureReason { get; set; }
        public string? SessionID { get; set; }
    }
}
