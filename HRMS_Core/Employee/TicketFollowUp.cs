using Microsoft.AspNetCore.Http;

namespace HRMS_Core.Employee
{
    public class TicketFollowUp
    {
        public int TicketFollowUpId { get; set; }
        public string? TicketFollowRemark { get; set; }
        public DateTime? TicketFollowDate { get; set; }
        public DateTime? NextTicketFollowDate { get; set; }
        public string? FollowUpDocumentUrl { get; set; }
        public string? TaggedUsers { get; set; }
        public int? TicketApplicationId { get; set; }
        public int? TicketStatusId { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public bool? IsEnabled { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? TicketStatusName { get; set; }
    }
    public class vmTicketFollowUp
    {
        public int TicketFollowUpId { get; set; }
        public string? TicketFollowRemark { get; set; }
        public DateTime? TicketFollowDate { get; set; }
        public DateTime? NextTicketFollowDate { get; set; }
        public string? FollowUpDocumentUrl { get; set; }
        public IFormFile? FollowUpDocumentFile { get; set; }
        public string? TaggedUsers { get; set; }
        public int? TicketApplicationId { get; set; }
        public int? TicketStatusId { get; set; }
        public int? EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public bool? IsEnabled { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
