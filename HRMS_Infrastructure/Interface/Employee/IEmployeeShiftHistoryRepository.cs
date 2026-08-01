using HRMS_Core.Employee;
using HRMS_Core.VM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeShiftHistoryRepository : IRepository<EmployeeShiftHistory>
    {
        Task<SP_Response> CreateEmployeeShiftHistory(EmployeeShiftHistory model);
        Task<SP_Response> UpdateEmployeeShiftHistory(EmployeeShiftHistory model);
        Task<SP_Response> DeleteEmployeeShiftHistory(DeleteRecordVM deleteRecord);
        Task<List<EmployeeShiftHistory>> GetEmployeeShiftHistoryByEmployeeId(int employeeId);
    }
}
