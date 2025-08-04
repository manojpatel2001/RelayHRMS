using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Ess.InOut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeInOutRepository:IRepository<EmployeeInOutRecord>
    {

        Task<VMCommonResult> CreateEmpInOut(vmInOut Record);
        Task<List<VMInOutRecord>> GetInOutRecord(int empid, string month, string year);             
        Task<List<VMInOutRecord>> GetMultipleInOutRecordAsync(int empid, string Month, string Year);
        Task<List<AttendanceInOutReportVM>> AttendanceMultipleInOutReport(int empid, string Month, string Year);
        Task<List<AttendanceInOutReportVM>> AttendancefirstInOutReport(int empid, string Month, string Year);
        Task<List<EmployeeInOutReportVM>> GetEmployeeInOutReport(EmployeeInOutFilterVM outFilterVM);
        Task<bool> UpdateEmployeeOutTimeAsync(int empId, DateTime forDate, DateTime outTime, string updatedBy);
        Task<bool> Update(AttendanceRegularization Record);
        Task<List<vmGetMonthlyAttendanceDetails>> GetMonthlyAttendanceDetails(vmInOutParameter vmInOutParameter);
        Task<List<vmGetMonthlyAttendanceLog>> GetMonthlyAttendanceLog(vmInOutParameter vmInOutParameter);
    }
}
