using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class LeaveCancellationReportRequest
    {
        public int? EmployeeId { get; set; }
        public int? CompanyId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int? LeaveTypeId { get; set; }
        public string? RecordType { get; set; }
    }
}
