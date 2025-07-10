using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeInOut:IRepository<EmployeeInOutRecord>
    {

        Task<VMCommonResult> CreateEmpInOut(EmployeeInOutRecord Record);
        Task<List<VMInOutRecord>> GetInOutRecord(int empid, string month, string year);             
        Task<List<VMInOutRecord>> GetMultipleInOutRecordAsync(int empid, string Month, string Year);
        Task<List<AttendanceInOutReportVM>> AttendanceMultipleInOutReport(int empid, string Month, string Year);
        Task<List<AttendanceInOutReportVM>> AttendancefirstInOutReport(int empid, string Month, string Year);
        Task<bool> UpdateEmployeeOutTimeAsync(int empId, DateTime forDate, DateTime outTime, string updatedBy);
    }
}
