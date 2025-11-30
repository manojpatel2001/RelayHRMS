using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Employee
{
    public class AttendanceRegularizationAdmin
    {
        public int? AttendanceRegularizationId { get; set; }
        public int? EmpId { get; set; }
        public string? FullName { get; set; }
        public string? EmployeeCode { get; set; }

        public DateTime? ForDate { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string? ShiftTime { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public decimal? Duration { get; set; }
        public string? Day { get; set; }

        public string? Reason { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
