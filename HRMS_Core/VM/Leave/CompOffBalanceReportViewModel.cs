using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class CompOffBalanceReportViewModel
    {
        public string? CompanyName { get; set; }
        public string? CompanyAddress { get; set; }
        public string? AsOnDate { get; set; } 
        public string? EmployeeCode { get; set; }
        public string? EmployeeName { get; set; } 
        public decimal? AvailBalance { get; set; }
        public decimal? Used { get; set; }
        public decimal? TotalBalance { get; set; }
        public decimal? AvailDays { get; set; }
        public int? Limit { get; set; }
        public string? CompOffType { get; set; }
        public decimal? RemainingDays { get; set; }
        public string? ForDate { get; set; } 
    }
}
