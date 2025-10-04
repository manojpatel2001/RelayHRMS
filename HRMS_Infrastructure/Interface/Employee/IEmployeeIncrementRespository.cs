using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeIncrementRespository
    {
        Task<List<IncrementReason>> GetAllIncrementReason();
        Task<APIResponse> CreateEmployeeIncrementSalary(vmEmployeeIncrementSalary vmEmployeeSalary);
        Task<APIResponse> GetAllIncrementEmployees(int companyId);
        Task<APIResponse> DeleteIncrement(int EmployeeId);
    }
}
