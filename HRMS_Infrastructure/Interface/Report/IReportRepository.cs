using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Leave;
using HRMS_Core.VM.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Report
{
    public interface IReportRepository
    {
        Task<List<LeaveBalanceViewModelForAdmin>> GetLeaveBalanceForAdmin(LeaveBalance_ParamForAdmin vm);
        Task<List<ActiveorInactiveUsers>> GetActiveOrInactiveUsers(string Action , int Compid);
        Task<List<MobileUserViewModel>> GetActiveOrInactiveMobileUsers(string Action, int Compid);
        Task<SP_Response> UpdateMobileUsers(UpdateMobileUserStatusRequest model);
    }
}
