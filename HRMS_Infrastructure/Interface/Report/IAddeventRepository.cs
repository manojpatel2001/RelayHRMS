using HRMS_Core.Report;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.ManagePermision;
using HRMS_Core.VM.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Report
{
    public interface IAddeventRepository:IRepository<AddEvent>
    {
        Task<SP_Response> CreateEvent(AddEvent model);
        Task<SP_Response> UpdateEvent(AddEvent model);
        Task<VMCommonResult> DeleteEvent(DeleteRecordVModel deleteRecord);
        Task<AddEvent?> GetEventById(vmCommonGetById filter);
        Task<List<EventModelVM>> GetAllEvent( DateTime TargetDate);
        Task<List<EventViewModel>> GetUserEvent(int UserId);
    }
}
