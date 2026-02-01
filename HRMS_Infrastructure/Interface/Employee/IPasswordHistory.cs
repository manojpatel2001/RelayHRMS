using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.PasswordHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IPasswordHistory:IRepository<PasswordHistory>
    {  
        Task<HRMSUserIdentity?> ChangePassword(PasswordHistory histroy);
         Task<VMCommonResult> CreateHistoryPassword(PasswordHistory history);
        Task<VMCommonResult> CheckLastPassword(PasswordHistory history);
         Task<SP_Response> ResetPassword(VMResetPassword vmResetPassword);
    }
}
