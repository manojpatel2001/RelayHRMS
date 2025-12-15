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
        public int EmplooyeId { get; set; } 
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
        public int LeaveApplicationId { get; set; }
        public int LeaveCancelReasonId { get; set; }
        public bool IsApproved { get; set; }
        public bool IsPending { get; set; }
        public bool IsRejected { get; set; }
    }
}
