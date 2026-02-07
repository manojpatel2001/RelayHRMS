using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.VM.OtherMaster
{
    public class InsertLeaveTransactionRequest
    {
        public int? EmpId { get; set; }
        public int? LeaveTypeId { get; set; }
        public string? TransactionType { get; set; } // "Credit" / "Debit"
        public decimal? LeaveValue { get; set; }     // e.g., 0.5, 2.0
        public string? Reason { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

}
