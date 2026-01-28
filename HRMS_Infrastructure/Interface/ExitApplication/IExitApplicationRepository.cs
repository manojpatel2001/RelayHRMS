using HRMS_Core.Employee;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.ExitApplication;
using HRMS_Core.VM.ManagePermision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.ExitApplication
{
    public interface IExitApplicationRepository : IRepository<ExitApplicationVm>
    {
        Task<SP_Response> CreateExitApplication(ExitApplicationVm model);
        Task<SP_Response> UpdateExitApplication(ExitApplicationVm model);
        Task<SP_Response> UpdateExitApproval(ExitApplicationUpdateparam model);
        Task<SP_Response> DeleteExitApplication(DeleteRecordVModel deleteRecord);
        Task<ExitApplicationReportVm?> GetExitApplicationById(int Employeeid);
        Task<List<GetExitApproval?>> GetExitApproval(ExitApprovalParam model);
        Task<List<ExitClearanceApprovalVM?>> GetExitClearanceApproval(ExitApplicationFilterModel model);

        Task<SP_Response> UpdateNOCFormFlag(int exitApplicationId);
    }
}
