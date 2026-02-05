using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class LeaveApprovalReportVM
    {
        public int? LeaveApplicationId { get; set; }
        public string? LeaveTypeName { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? No_Of_Date { get; set; }
        public string? Reason { get; set; }
        public string? ApplicationType { get; set; }
        public string? LeaveStatus { get; set; }
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
    }
}
