using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.importData;
using Microsoft.SqlServer.Server;
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
        Task<List<AttendanceRegularization>> SoftDelete(DeleteRecordVModel DeleteRecord);
        Task<List<AttendanceRegularizationVM>> GetAttendanceRegularization(AttendanceRegularizationSearchFilterVM attendance);
        Task<bool> Update(AttendanceRegularization Record ,int empInOutId);
        Task<List<EmpInOutVM>> GetEmployeeInOut( int? EmpId , DateTime? ForDate);
        Task<VMCommonResult> Create(AttendanceRegularization model);
        Task<VMCommonResult> Update(AttendanceRegularization model);
        Task<VMCommonResult> Delete(DeleteRecordVModel deleteRecord);
    }
}
