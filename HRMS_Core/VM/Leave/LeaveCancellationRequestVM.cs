using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.Leave
{
    public class LeaveCancellationRequestVM
    {
        public int LeaveCancellationId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int NoOfDate { get; set; }
        public string? Reason { get; set; }
        public int LeaveCancelReasonId { get; set; }
        public string? LeaveStatus { get; set; }
        public int LeaveTypeId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public bool IsApproved { get; set; } = false;
        public bool IsRejected { get; set; } = false;
    }
}
