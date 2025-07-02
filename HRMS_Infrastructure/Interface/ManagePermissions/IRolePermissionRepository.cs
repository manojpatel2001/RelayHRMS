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
    public interface IRolePermissionRepository:IRepository<RolePermission>
    {
        Task<List<vmGetAllRolesWithPermissionByCompanyId>> GetAllRolesWithPermissionByCompanyId(int CompanyId);
        Task<List<vmGetAllPermissionByRoleId>> GetAllPermissionByRoleId(vmRoleManagePermission vmRole);
        Task<VMCommonResult> CreateRolePermission(RolePermission permission);
        Task<VMCommonResult> UpdateRolePermission(RolePermission permission);
        Task<VMCommonResult> DeleteRolePermission(vmRoleManagePermission delete);
        Task<List<RoleManagePermissionDto>> GetAllRolesWithPermissionByRoleId(vmRoleManagePermission vmRole);
        Task<List<vmGetEmployeeRolesAndPermissions>> GetEmployeeRolesAndPermissions(int EmployeeId);
    }
}
