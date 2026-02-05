using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;


namespace HRMS_Infrastructure.Interface.OtherMaster
{
    public interface ITicketPriorityRepository 
    {
        Task<List<TicketPriority>> GetAllTicketPriority(int companyId);
        Task<TicketPriority?> GetTicketPriorityById(int ticketPriorityId);
        Task<SP_Response> CreateTicketPriority(TicketPriority ticketPriority);
        Task<SP_Response> UpdateTicketPriority(TicketPriority ticketPriority);
        Task<SP_Response> DeleteTicketPriority(DeleteRecordVM deleteRecord);
    }
}
