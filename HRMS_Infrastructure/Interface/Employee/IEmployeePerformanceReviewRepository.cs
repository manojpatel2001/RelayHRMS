using HRMS_Core.Employee;
using HRMS_Core.VM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeePerformanceReviewRepository : IRepository<EmployeePerformanceReview>
    {
        Task<SP_Response> CreateEmployeePerformanceReview(EmployeePerformanceReview model);
        Task<SP_Response> UpdateEmployeePerformanceReview(EmployeePerformanceReview model);
        Task<SP_Response> DeleteEmployeePerformanceReview(DeleteRecordVM deleteRecord);
        Task<List<EmployeePerformanceReview>> GetEmployeePerformanceReviewByEmployeeId(int employeeId);
    }
}
