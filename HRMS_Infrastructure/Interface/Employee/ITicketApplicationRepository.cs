using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface ITicketApplicationRepository
    {
        Task<List<TicketApplication>> GetAllTicketApplications(CommonParameter common);
        Task<TicketApplication?> GetTicketApplicationById(int ticketApplicationId);
        Task<SP_Response> CreateTicketApplication(vmTicketApplication ticketApplication);
        Task<SP_Response> UpdateTicketApplication(vmTicketApplication ticketApplication);
        Task<SP_Response> DeleteTicketApplication(DeleteRecordVM deleteRecord);
        Task<(List<vmEmployeeListDto> Employees, List<vmTicketTypeDto> TicketTypes)> GetEmployeeAndTicketTypeByDepartmentId(CommonParameter commonParameter);
        Task<List<TicketApplication>> GetAllAssignTicketApplications(CommonParameter common);

    }
}
