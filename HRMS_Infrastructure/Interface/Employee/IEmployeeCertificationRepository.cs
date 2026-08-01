using HRMS_Core.Employee;
using HRMS_Core.VM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeCertificationRepository : IRepository<EmployeeCertification>
    {
        Task<SP_Response> CreateEmployeeCertification(EmployeeCertification model);
        Task<SP_Response> UpdateEmployeeCertification(EmployeeCertification model);
        Task<SP_Response> DeleteEmployeeCertification(DeleteRecordVM deleteRecord);
        Task<List<EmployeeCertification>> GetEmployeeCertificationByEmployeeId(int employeeId);
    }
}
