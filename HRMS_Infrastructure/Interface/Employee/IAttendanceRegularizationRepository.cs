using HRMS_Core.DbContext;
using HRMS_Core.Employee;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.importData;
using HRMS_Core.VM.Report;
using HRMS_Core.VM.Salary;
using HRMS_Utility;
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
        Task<List<AttendanceRegularizationVM>> GetAttendanceRegularizationApproval(AttendanceRegularizationSearchFilterVM attendance);
        Task<List<AttendanceRegularizationAdmin>> GetAttendanceRegularizationForAdmin(AttendanceRegularizationSearchFilterForAdminVM attendance);
        Task<List<AttendanceRegularizationAdmin>> GetAttendanceRequestAdminReport(AttendanceRequestReportFilterVm attendance);
        Task<bool> Update(AttendanceRegularization Record ,int empInOutId);
        Task<List<EmpInOutVM>> GetEmployeeInOut( int? EmpId , DateTime? ForDate);
        Task<List<EMpDetails>> GetEmployeeDetails( int? EmpId );
        Task<List<AttendanceCount>> GetEmployeeAttendanceRequestsCountForCurrentMonth( int? EmpId ,int Month ,int year);
        Task<APIResponse> Create(AttendanceRegularization model);
        Task<APIResponse> Update(AttendanceRegularization model);
        Task<APIResponse> Delete(DeleteRecordVModel deleteRecord);
        Task<List<AttendanceDetails>> GetAttendanceDetails(EmployeeInOutFilterVM outFilterVM);
        Task<List<LimitedReasonvm>> GetAttendanceReasonsByLimitType();
    }
}
