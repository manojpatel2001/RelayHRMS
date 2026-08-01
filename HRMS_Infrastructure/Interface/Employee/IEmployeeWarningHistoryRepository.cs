using HRMS_Core.Employee;
using HRMS_Core.VM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeWarningHistoryRepository : IRepository<EmployeeWarningHistory>
    {
        Task<SP_Response> CreateEmployeeWarningHistory(EmployeeWarningHistory model);
        Task<SP_Response> UpdateEmployeeWarningHistory(EmployeeWarningHistory model);
        Task<SP_Response> DeleteEmployeeWarningHistory(DeleteRecordVM deleteRecord);
        Task<List<EmployeeWarningHistory>> GetEmployeeWarningHistoryByEmployeeId(int employeeId);
    }
}
