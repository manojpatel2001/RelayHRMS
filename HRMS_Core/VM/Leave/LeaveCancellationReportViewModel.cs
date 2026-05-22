using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class LeaveCancellationReportViewModel
    {

        public int? LeaveApplicationid { get; set; }
        public int? Id { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public string? Leave_Name { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? No_Of_Date { get; set; }
        public string? Reason { get; set; }
        public string? LeaveStatus { get; set; }
        public int? Leave_TypeId { get; set; }
    }
}
