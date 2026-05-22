namespace HRMS_Core.VM.Ess.InOut
{
    public class vmInOutParameter
    {
        public int? MonthNumber { get; set; } = 0;
        public int? year { get; set; } = 0;
        public int? CompanyId { get; set; }= 0;
        public int? EmployeeId { get; set; } = 0;
        public DateTime? ToDate { get; set; } = DateTime.Now;
        public DateTime? FromDate { get; set; } = DateTime.Now;
        public int? MemberId { get; set; } = 0;
        
    }
}
