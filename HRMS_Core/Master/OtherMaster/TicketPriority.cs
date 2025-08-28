

namespace HRMS_Core.Master.OtherMaster
{
    
    public class TicketPriority
    {
        public int TicketPriorityId { get; set; }
        public string? TicketPriorityName { get; set; }
        public string? EscalationHours { get; set; }
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
