using HRMS_Core.Employee;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.JobMaster;
using HRMS_Core.VM.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IAttendanceLockRepository : IRepository<AttendanceLock>
    {
        Task<List<EmployeeLockStatusViewModel>> GetEmployeeLockStatus(AttendanceLockParamVm model);
    }
}
