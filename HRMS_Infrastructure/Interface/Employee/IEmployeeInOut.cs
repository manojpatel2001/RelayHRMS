using HRMS_Core.Employee;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.Employee
{
    public interface IEmployeeInOut:IRepository<EmployeeInOutRecord>
    {

        Task<VMCommonResult> CreateEmpInOut(EmployeeInOutRecord Record);
    }
}
