using HRMS_Core.Employee;
using HRMS_Core.Master.JobMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Ess.InOut;
using HRMS_Core.VM.Report;
using HRMS_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeInOutRepository:IRepository<EmployeeInOutRecord>
    {

        Task<APIResponse> CreateEmpInOut(vmInOut Record);
        Task<VMCommonResult> CreateAttendanceDetails(AttendanceDetailsViewModel Record);
        Task<List<VMInOutRecord>> GetInOutRecord(int empid, string month, string year);             
        Task<List<VMInOutRecord>> GetMultipleInOutRecordAsync(int empid, string Month, string Year);
        Task<List<AttendanceInOutReportVM>> AttendanceMultipleInOutReport(int empid, string Month, string Year);
        Task<List<AttendanceInOutReportVM>> AttendancefirstInOutReport(int empid, string Month, string Year);
        Task<APIResponse> GetEmployeeInOutReport(EmployeeInOutFilterVM outFilterVM);

        Task<bool> UpdateEmployeeOutTimeAsync(int empId, DateTime forDate, DateTime outTime, string updatedBy);
        Task<VMCommonResult> UpdateAttendanceDetails(AttendanceDetailsViewModel model);
        Task<List<vmGetMonthlyAttendanceDetails>> GetMonthlyAttendanceDetails(vmInOutParameter vmInOutParameter);
        Task<List<vmGetMonthlyAttendanceLog>> GetMonthlyAttendanceLog(vmInOutParameter vmInOutParameter);
        Task<List<vmGetMonthlyAttendanceDetails>> GetDateWiseAttendanceDetails(vmInOutParameter vmInOutParameter);
        Task<List<vmGetEmployeesByReportingManager>> GetEmployeesByReportingManager(int EmployeeId);
        Task<List<EmpInOutReportforAdmin>> GetEmpInOutReportForAdmin(EmpInOutReportFilter filter);
        Task<APIResponse> GetAttendanceRegularizationAlerts(CommonParameter model);
    }
}
