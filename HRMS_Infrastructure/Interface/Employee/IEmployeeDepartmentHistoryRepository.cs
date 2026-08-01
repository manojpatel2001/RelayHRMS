using HRMS_Core.Employee;
using HRMS_Core.VM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeDepartmentHistoryRepository : IRepository<EmployeeDepartmentHistory>
    {
        Task<SP_Response> CreateEmployeeDepartmentHistory(EmployeeDepartmentHistory model);
        Task<SP_Response> UpdateEmployeeDepartmentHistory(EmployeeDepartmentHistory model);
        Task<SP_Response> DeleteEmployeeDepartmentHistory(DeleteRecordVM deleteRecord);
        Task<List<EmployeeDepartmentHistory>> GetEmployeeDepartmentHistoryByEmployeeId(int employeeId);
    }
}
