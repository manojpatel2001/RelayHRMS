using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Report
{
    public class VMYearlyLeaveReport
    {
        public string? Location { get; set; }
        public string? EmployeeCode { get; set; }
        public string? Name { get; set; }
        public DateTime? DOJ { get; set; }

        public string? LeaveType { get; set; }
        public decimal? Opening { get; set; }
        public string? RowType { get; set; }

        public decimal? Jan { get; set; }
        public decimal? Feb { get; set; }
        public decimal? Mar { get; set; }
        public decimal? Apr { get; set; }
        public decimal? May { get; set; }
        public decimal? Jun { get; set; }
        public decimal? Jul { get; set; }
        public decimal? Aug { get; set; }
        public decimal? Sep { get; set; }
        public decimal? Oct { get; set; }
        public decimal? Nov { get; set; }
        public decimal? Dec { get; set; }
        public decimal? Closing { get; set; }

        // ── Fix: back to int (SP is casting to INT) ──
        public int EmpRowSpan { get; set; }
        public int RowNumInGroup { get; set; }
        public int LeaveTypeRowSpan { get; set; }
        public int RowNumInLeaveType { get; set; }
    }
}
