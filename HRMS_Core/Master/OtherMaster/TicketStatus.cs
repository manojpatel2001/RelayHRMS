using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Core.Master.OtherMaster
{
    public class TicketStatus
    {
        public int TicketStatusId {  get; set; }
        public string? TicketStatusName {  get; set; }
        public bool? IsEnabled { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;
    }
      
    

    
    public class TicketApplication
    {
        public int TicketApplicationId { get; set; }
        public DateTime? TicketDate { get; set; }
        public int? DepartmentId { get; set; }
        public int? TicketTypeId { get; set; }
        public string? TicketAssignId { get; set; }
        public int? EmployeeId { get; set; }
        public string? TicketDescription { get; set; }
        public int? TicketPriority { get; set; }
        public string? AttachDocumentUrl { get; set; }
        public int? TicketStatusId { get; set; }
        public bool? IsEnabled { get; set; } = true;
        public bool? IsDeleted { get; set; } = false;
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public string? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
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
