using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.ExitApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.ExitApplication
{
    public interface IExitApplicationRepository:IRepository<ExitApplicationVm>
    {
        Task<SP_Response> CreateExitApplication(ExitApplicationVm model);
        Task<SP_Response> UpdateExitApplication(ExitApplicationVm model);
        Task<SP_Response> DeleteExitApplication(DeleteRecordVM deleteRecord);
    }
}
