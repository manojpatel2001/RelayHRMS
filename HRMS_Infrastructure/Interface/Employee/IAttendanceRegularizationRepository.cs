using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IAttendanceRegularizationRepository:IRepository<AttendanceRegularization>
    {
        Task<bool> UpdateAttendanceRegularization(AttendanceRegularization attendanceRegularization);
        Task<AttendanceRegularization> SoftDelete(DeleteRecordVM DeleteRecord);
    }
}
