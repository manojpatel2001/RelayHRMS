using HRMS_Core.VM.OtherMaster;
using HRMS_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.OtherMaster
{
    public interface ILeaveTransactionRepository
    {
        Task<APIResponse> InsertLeaveTransaction(InsertLeaveTransactionRequest request);
        Task<APIResponse> GetAllLeaveTypeByCompanyId(int companyId);
        Task<APIResponse> GetLeaveBalanceReport(int companyId);
    }
}
