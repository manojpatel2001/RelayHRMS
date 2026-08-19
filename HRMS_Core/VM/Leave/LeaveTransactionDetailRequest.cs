namespace HRMS_Core.VM.Leave
{
    public class LeaveTransactionDetailRequest
    {
        public int EmpId { get; set; }
        public string LeaveType { get; set; }
        public string TransactionType { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
    }
}
