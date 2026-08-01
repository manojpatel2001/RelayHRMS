using HRMS_Core.Employee;
using HRMS_Core.VM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeTrainingRepository : IRepository<EmployeeTraining>
    {
        Task<SP_Response> CreateEmployeeTraining(EmployeeTraining model);
        Task<SP_Response> UpdateEmployeeTraining(EmployeeTraining model);
        Task<SP_Response> DeleteEmployeeTraining(DeleteRecordVM deleteRecord);
        Task<List<EmployeeTraining>> GetEmployeeTrainingByEmployeeId(int employeeId);
    }
}
