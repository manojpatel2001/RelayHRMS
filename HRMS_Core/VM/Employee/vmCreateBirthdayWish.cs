namespace HRMS_Core.VM.Employee
{
    public class vmCreateBirthdayWish
    {
        public int BirthdayEmployeeId { get; set; }
        public int WishedByEmployeeId { get; set; }
        public int CompanyId { get; set; }
        public string? Message { get; set; }
    }
}
