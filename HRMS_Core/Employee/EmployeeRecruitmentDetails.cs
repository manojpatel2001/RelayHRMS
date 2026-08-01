using System;

namespace HRMS_Core.Employee
{
    public class EmployeeRecruitmentDetails
    {
        public int EmployeeRecruitmentDetailsId { get; set; }
        public int EmployeeId { get; set; }
        public DateTime? ApplicationDate { get; set; }
        public DateTime? InterviewDate { get; set; }
        public string? InterviewerName { get; set; }
        public string? Source { get; set; }
        public DateTime? OfferDate { get; set; }
        public DateTime? OfferAcceptedDate { get; set; }
        public string? Remarks { get; set; }
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
