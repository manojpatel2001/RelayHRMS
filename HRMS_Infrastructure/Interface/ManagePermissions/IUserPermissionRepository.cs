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
    }
}
