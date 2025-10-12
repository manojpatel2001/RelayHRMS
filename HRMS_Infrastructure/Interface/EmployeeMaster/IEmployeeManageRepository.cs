using HRMS_Core.DbContext;
using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Utility;
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
        Task<List<vmGetAllEmployee>> GetAllEmployeeActiveOrLeft(bool IsLeft, int companyId);
        Task<vmGetEmployeeById?> GetEmployeeById(int Id);
        Task<List<vmUpdateEmployee>> GetAllEmployeeForUpdate(int companyId);
        Task<VMCommonResult> UpdateEmployee(vmUpdateEmployee employee);
        Task<VMCommonResult> DeleteEmployee(DeleteRecordVM deleteRecord);
        Task<VMCommonResult> UpdateEmployeeProfileAndSignature(vmUpdateEmployeeProfile model);
        Task<vmGetNextEmployeeCode?> GetNextEmployeeCode(int CompanyId);
        Task<VMGetExistEmployeeCode?> GetExistEmployeeCode(vmCommonParameters vmCommonParameters);
        Task<VMCommonResult> UpdateLastLogin(int empid, int compId);
        Task<List<EmployeePersonalInformationVM>> EmployeePersonalInformation(int empid, int compId);
        Task<vmUserLogin?> UserLogin(vmLogin login);
        Task<List<vmGetAllEmployee_DropDown>> GetAllEmployee_DropDown(int companyId ,string BranchId);
        Task<APIResponse> GetRecordsForUpdate(int CompanyId);
        Task<APIResponse> GetRecordsForAdd(int CompanyId);
        Task<APIResponse> GetReportingList();
        Task<APIResponse> GetEmployeeListByBranchId(int BranchId);
    }
}
