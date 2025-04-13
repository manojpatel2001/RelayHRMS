using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM.OtherMaster;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.OtherMaster
{
    public interface ITicketTypeRepository:IRepository<TicketType>
    {
        Task<VMCommonResult> CreateTicketType(TicketType ticketType);
        Task<VMCommonResult> UpdateTicketType(TicketType ticketType);
        Task<VMCommonResult> DeleteTicketType(DeleteRecordVM deleteRecordVM);
        Task<List<vmGetAllTicketTypes>> GetAllTicketTypes();
    }
}
