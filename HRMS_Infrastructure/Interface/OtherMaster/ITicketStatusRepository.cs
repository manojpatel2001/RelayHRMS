using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.OtherMaster
{
    public interface ITicketStatusRepository
    {
        Task<List<TicketStatus>> GetAllTicketStatus();
        Task<TicketStatus?> GetTicketStatusById(int ticketStatusId);
        Task<SP_Response> CreateTicketStatus(TicketStatus ticketStatus);
        Task<SP_Response> UpdateTicketStatus(TicketStatus ticketStatus);
        Task<SP_Response> DeleteTicketStatus(DeleteRecordVM deleteRecord);
        Task<TicketStatus?> GetTicketStatusByName(string ticketStatusName);
    }
}
