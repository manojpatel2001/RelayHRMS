using System;

namespace HRMS_Core.VM.Leave
{
    public class LeaveTransactionDetailViewModel
    {
        public int? LeaveApplicationId { get; set; }
        public DateTime? TransactionDate { get; set; }
        public decimal? Days { get; set; }
        public string? TransactionReason { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? ApplicationReason { get; set; }
        public string? LeaveStatus { get; set; }
    }
}
