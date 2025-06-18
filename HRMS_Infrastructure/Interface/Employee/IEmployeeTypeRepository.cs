using HRMS_Core.Employee;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeTypeRepository:IRepository<EmployeeType>
    {
        Task<VMCommonResult> CreateEmployeeType(EmployeeType employeeType);
        Task<VMCommonResult> UpdateEmployeeType(EmployeeType employeeType);
        Task<VMCommonResult> DeleteEmployeeType(DeleteRecordVM employeeType);
        Task<EmployeeType?> GetEmployeeTypeById(int employeeTypeId);
        Task<List<EmployeeType>> GetAllEmployeeTypes();
    }
}
