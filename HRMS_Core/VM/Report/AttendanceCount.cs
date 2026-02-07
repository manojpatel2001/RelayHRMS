using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class AttendanceCount
    {
        public int EmpId { get; set; }
        public int TotalRequests { get; set; }
        public int LimitedRequestsCount { get; set; }
        public int UnlimitedRequestsCount { get; set; }
    }
}
