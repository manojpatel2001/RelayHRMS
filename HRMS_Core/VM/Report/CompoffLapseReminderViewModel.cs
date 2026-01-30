using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class CompoffLapseReminderViewModel
    {
        public string? EmployeeCode { get; set; }
        public string? FullName { get; set; }
        public string? DesignationName { get; set; }
        public DateTime? ForDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int? Balance { get; set; }
    }
}
