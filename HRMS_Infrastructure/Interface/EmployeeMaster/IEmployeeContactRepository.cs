using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.EmployeeMaster
{
    public interface IEmployeeContactRepository:IRepository<EmployeeContact>
    {
        Task<VMCommonResult> CreateEmployeeContact(EmployeeContact contact);

        Task<VMCommonResult> UpdateEmployeeContact(EmployeeContact contact);

        Task<VMCommonResult> DeleteEmployeeContact(DeleteRecordVM contact);

        Task<EmployeeContact?> GetEmployeeContactByEmployeeId(string employeeId);

        Task<EmployeeContact?> GetEmployeeContactById(int employeeContactId);

    }
}
