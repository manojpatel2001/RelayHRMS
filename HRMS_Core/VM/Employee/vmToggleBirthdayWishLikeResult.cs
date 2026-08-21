namespace HRMS_Core.VM.Employee
{
    public class vmToggleBirthdayWishLikeResult
    {
        public bool IsLikedNow { get; set; }
        public int? WishedByEmployeeId { get; set; }
        public int? BirthdayEmployeeId { get; set; }
    }
}
