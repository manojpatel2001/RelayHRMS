using HRMS_Core.Employee;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface ITicketFollowUpRepository
    {
        Task<List<TicketFollowUp>> GetAllTicketFollowUpByApplicationId(int ticketApplicationId);
        Task<TicketFollowUp?> GetTicketFollowUpById(int ticketFollowUpId);
        Task<SP_Response> CreateTicketFollowUp(vmTicketFollowUp ticketFollowUp);
        Task<SP_Response> UpdateTicketFollowUp(vmTicketFollowUp ticketFollowUp);
        Task<SP_Response> DeleteTicketFollowUp(DeleteRecordVM deleteRecord);
    }
}
