using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace HRMS_Core.Employee
{
    public class TicketApplication
    {
        public int TicketApplicationId { get; set; }
        public DateTime? TicketDate { get; set; }
        public int? DepartmentId { get; set; }
        public int? TicketTypeId { get; set; }
        public int? TicketAssignId { get; set; }
        public int? EmployeeId { get; set; }
        public string? TicketDescription { get; set; }
        public int? TicketPriorityId { get; set; }
        public string? AttachDocumentUrl { get; set; }
        public int? TicketStatusId { get; set; }
        public int? CompanyId { get; set; }
        public bool? IsEnabled { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? DepartmentName { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeCode { get; set; }
        public string? TicketPriorityName { get; set; }
        public string? AssignTo { get; set; }
        public string? AssignToCode { get; set; }
        public string? TicketStatusName { get; set; }
        public string? TicketTypeName { get; set; }
        public string? EscalationHours { get; set; }
    }
    public class vmTicketApplication
    {
        public int TicketApplicationId { get; set; }
        public DateTime? TicketDate { get; set; }
        public int? DepartmentId { get; set; }
        public int? TicketTypeId { get; set; }
        public int? TicketAssignId { get; set; }
        public int? EmployeeId { get; set; }
        public string? TicketDescription { get; set; }
        public int? TicketPriorityId { get; set; }
        public string? AttachDocumentUrl { get; set; }
        public IFormFile? AttachDocumentFile { get; set; }
        public int? TicketStatusId { get; set; }
        public int? CompanyId { get; set; }
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
