using HRMS_Core.Employee;
using HRMS_Core.VM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeAwardRepository : IRepository<EmployeeAward>
    {
        Task<SP_Response> CreateEmployeeAward(EmployeeAward model);
        Task<SP_Response> UpdateEmployeeAward(EmployeeAward model);
        Task<SP_Response> DeleteEmployeeAward(DeleteRecordVM deleteRecord);
        Task<List<EmployeeAward>> GetEmployeeAwardByEmployeeId(int employeeId);
    }
}
