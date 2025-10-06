using HRMS_Core.VM.Leave;
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
    }
}
