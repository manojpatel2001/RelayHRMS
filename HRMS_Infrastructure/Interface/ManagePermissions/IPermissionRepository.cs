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
    public interface IPermissionRepository : IRepository<Permission>
    {
        Task<List<Permission>> GetAllPermissions();
        Task<Permission?> GetPermissionById(vmCommonGetById vmCommonGetById);
        Task<VMCommonResult> CreatePermission(Permission permission);
        Task<VMCommonResult> UpdatePermission(Permission permission);
        Task<VMCommonResult> DeletePermission(DeleteRecordVM deleteRecord);
        Task<List<GroupedPermissionDto>> GetAllGroupPermissionsAsync();
    }

}
