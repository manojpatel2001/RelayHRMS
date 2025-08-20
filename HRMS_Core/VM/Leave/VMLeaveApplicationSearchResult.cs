using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class VMLeaveApplicationSearchResult
    {

        public int LeaveApplicationId { get; set; }
        public string? LeaveTypeName { get; set; }
        public string? EmployeeName { get; set; }
        public string? ReportingPersonName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? Todate { get; set; }
        public decimal? No_Of_Date { get; set; }
        public string? Reason { get; set; }
        public string? ApplicationType { get; set; }
        public string? LeaveStatus { get; set; }
    }
}
