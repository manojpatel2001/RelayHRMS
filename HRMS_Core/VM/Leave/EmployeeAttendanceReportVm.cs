using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class EmployeeAttendanceReportVm
    {
        public string? BranchName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }    
        public Dictionary<string, string> Days { get; set; }
        public decimal TotalP { get; set; }
        public decimal TotalA { get; set; }
        public decimal TotalW { get; set; }
        public decimal TotalL { get; set; }
        public decimal TotalH { get; set; }
        public decimal TotalHFLeave { get; set; }
        public decimal TotalCO { get; set; }
        public decimal TotalLWP { get; set; }
        public decimal TotalPayableDays { get; set; }
        public decimal TotalUnpaidDays { get; set; }
        public decimal TotalMonthDays { get; set; }
    }
}
