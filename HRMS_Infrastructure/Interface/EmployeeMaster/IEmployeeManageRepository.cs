using HRMS_Core.DbContext;
using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.EmployeeMaster
{
    public interface IEmployeeManageRepository:IRepository<HRMSUserIdentity>
    {
        Task<List<vmGetAllEmployee>> GetAllEmployee(int companyId);
        Task<List<vmGetAllEmployee>> GetAllEmployeeByIsBlocked(bool IsBlocked,int companyId);
        Task<HRMSUserIdentity?> GetEmployeeById(string Id);
        Task<VMEmpResult> CreateEmployee(vmEmployeeData employee);
        Task<VMEmpResult> UpdateEmployee(vmEmployeeData employee);
        Task<VMEmpResult> DeleteEmployee(DeleteRecordVM deleteRecord);
    }
}
