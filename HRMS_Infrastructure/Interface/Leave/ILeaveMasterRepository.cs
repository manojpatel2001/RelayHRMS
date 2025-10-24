using HRMS_Core.Leave;
using HRMS_Core.VM.Leave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Leave
{
    public interface ILeaveMasterRepository:IRepository<LeaveMaster>
    {
        Task<List<LeaveMaster>> GetLeaveMaster(int CompId);
        Task<List<LeaveTypeViewModel>> GetLeaveTypesForEmployee(int CompId ,int Empid);
    }
}
