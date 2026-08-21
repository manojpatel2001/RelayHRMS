using System;

namespace HRMS_Core.VM.Employee
{
    public class vmBirthdayWishComment
    {
        public int BirthdayWishCommentId { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? ProfileUrl { get; set; }
        public string? CommentText { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
