using System;

namespace HRMS_Core.VM.Employee
{
    public class vmBirthdayWish
    {
        public int BirthdayWishId { get; set; }
        public int WishedByEmployeeId { get; set; }
        public string? WishedByName { get; set; }
        public string? WishedByProfileUrl { get; set; }
        public string? Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public bool IsLikedByMe { get; set; }
    }
}
