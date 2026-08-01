using HRMS_Core.Employee;
using HRMS_Core.VM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeDesignationHistoryRepository : IRepository<EmployeeDesignationHistory>
    {
        Task<SP_Response> CreateEmployeeDesignationHistory(EmployeeDesignationHistory model);
        Task<SP_Response> UpdateEmployeeDesignationHistory(EmployeeDesignationHistory model);
        Task<SP_Response> DeleteEmployeeDesignationHistory(DeleteRecordVM deleteRecord);
        Task<List<EmployeeDesignationHistory>> GetEmployeeDesignationHistoryByEmployeeId(int employeeId);
    }
}
