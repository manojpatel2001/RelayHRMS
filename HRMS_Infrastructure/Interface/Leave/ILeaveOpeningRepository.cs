using HRMS_Core.Leave;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Leave
{
    public interface ILeaveOpeningRepository : IRepository<LeaveOpening>
    {

        Task<bool> UpdateleaveOpening(LeaveOpening leaveOpening);
        Task<LeaveOpening> SoftDelete(DeleteRecordVM DeleteRecord);
    }
}
