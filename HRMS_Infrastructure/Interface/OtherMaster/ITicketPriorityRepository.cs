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
    public interface ITicketPriorityRepository:IRepository<TicketPriority>
    {
        Task<VMCommonResult> CreateTicketPriority(TicketPriority ticketPriority);
        Task<VMCommonResult> UpdateTicketPriority(TicketPriority ticketPriority);
        Task<VMCommonResult> DeleteTicketPriority(DeleteRecordVM deleteRecordVM);
    }
}
