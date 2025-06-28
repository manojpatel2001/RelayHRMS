using HRMS_Core.ManagePermission;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.ManagePermissions
{
    public interface IRolePermissionRepository:IRepository<RolePermission>
    {
        Task<VMCommonResult> CreateRolePermission(RolePermission permission);
        Task<VMCommonResult> UpdateRolePermission(RolePermission permission);
        Task<VMCommonResult> DeleteRolePermission(DeleteRecordVM delete);
    }
}
