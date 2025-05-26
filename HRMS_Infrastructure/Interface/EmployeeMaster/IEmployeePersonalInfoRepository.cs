using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.EmployeeMaster
{
    public interface IEmployeePersonalInfoRepository : IRepository<EmployeePersonalInfo>
    {
        Task<List<EmployeePersonalInfo>> GetAllEmployeePersonalInfo();
        Task<EmployeePersonalInfo?> GetEmployeePersonalInfoByEmployeeId(int EmployeeId);
        Task<EmployeePersonalInfo?> GetEmployeePersonalInfoById(int employeePersonalInfoId);
        Task<VMCommonResult> CreateEmployeePersonalInfo(EmployeePersonalInfo employeePersonalInfo);
        Task<VMCommonResult> UpdateEmployeePersonalInfo(EmployeePersonalInfo employeePersonalInfo);
        Task<VMCommonResult> DeleteEmployeePersonalInfo(DeleteRecordVM deleteRecordVM);
    }
}
