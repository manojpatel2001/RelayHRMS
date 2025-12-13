using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class EmpLeaveCancellationRequestReportViewModel
    {
        public int LeaveCancellationId { get; set; }
        public int Id { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public string? Leave_Name { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int NoOfDate { get; set; }
        public string? Reason { get; set; }
        public string? ApprovedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? LeaveStatus { get; set; }
        public bool IsApproved { get; set; }
        public bool IsPending { get; set; }
        public bool IsRejected { get; set; }
    }
}
