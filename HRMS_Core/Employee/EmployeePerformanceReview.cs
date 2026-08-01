using System;

namespace HRMS_Core.Employee
{
    public class EmployeePerformanceReview
    {
        public int EmployeePerformanceReviewId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? ReviewPeriodStart { get; set; }
        public DateTime? ReviewPeriodEnd { get; set; }
        public DateTime? ReviewDate { get; set; }
        public int? ReviewerId { get; set; }
        public int? Rating { get; set; }
        public string? Strengths { get; set; }
        public string? AreasOfImprovement { get; set; }
        public string Status { get; set; } = "Draft";
        public bool IsEnabled { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
