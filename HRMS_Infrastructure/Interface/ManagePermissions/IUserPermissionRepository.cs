using HRMS_Core.ManagePermission;
using HRMS_Core.VM;
using HRMS_Core.VM.ManagePermision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.ManagePermissions
{
    public interface IUserPermissionRepository
    {
        Task<VMCommonResult> CreateUserPermission(UserPermission permission);
        Task<List<vmGetAllEmployeeListByCompanyId>> GetAllEmployeeListByCompanyId(int CompanyId);
        Task<List<vmGetAllUserWithPermissionByCompanyId>> GetAllUserWithPermissionByCompanyId(int CompanyId);
        Task<List<vmGetAllPermissionByEmployeeId>> GetAllPermissionByEmployeeId(vmRoleManagePermission permission);
        Task<VMCommonResult> DeleteUserPermission(vmRoleManagePermission delete);

        Task<VMCommonResult> CreateUserPermissionOverride(vmUserPermissionOverride permission);
        Task<VMCommonResult> RemoveUserPermissionOverride(vmUserPermissionOverride permission);
        Task<VMCommonResult> ResetUserPermissionOverrides(vmRoleManagePermission model);
        Task<List<vmGetUserPermissionOverride>> GetUserPermissionOverrides(vmRoleManagePermission model);
        Task<vmEmployeeRole?> GetEmployeeRole(int employeeId, int companyId);
    }
}
