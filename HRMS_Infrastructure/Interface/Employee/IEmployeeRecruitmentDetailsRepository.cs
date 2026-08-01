using HRMS_Core.Employee;
using HRMS_Core.VM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeRecruitmentDetailsRepository : IRepository<EmployeeRecruitmentDetails>
    {
        Task<SP_Response> CreateEmployeeRecruitmentDetails(EmployeeRecruitmentDetails model);
        Task<SP_Response> UpdateEmployeeRecruitmentDetails(EmployeeRecruitmentDetails model);
        Task<SP_Response> DeleteEmployeeRecruitmentDetails(DeleteRecordVM deleteRecord);
        Task<List<EmployeeRecruitmentDetails>> GetEmployeeRecruitmentDetailsByEmployeeId(int employeeId);
    }
}
