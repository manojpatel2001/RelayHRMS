using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM.OtherMaster;
using HRMS_Core.VM;


namespace HRMS_Infrastructure.Interface.OtherMaster
{
    public interface ITicketTypeRepository 
    {
        Task<SP_Response> CreateTicketType(TicketType ticketType);
        Task<SP_Response> UpdateTicketType(TicketType ticketType);
        Task<SP_Response> DeleteTicketType(DeleteRecordVM deleteRecordVM);
        Task<List<TicketType>> GetAllTicketTypes(int companyId);
        Task<TicketType?> GetTicketTypeById(int ticketTypeId);
    }
}
