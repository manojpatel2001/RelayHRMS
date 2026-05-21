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
    public interface IPermissionRepository 
    {
        Task<List<Permission>> GetAllPermissions();
        Task<Permission?> GetPermissionById(vmCommonGetById vmCommonGetById);
        Task<SP_Response> CreatePermission(Permission permission);
        Task<SP_Response> UpdatePermission(Permission permission);
        Task<SP_Response> DeletePermission(DeleteRecordVM deleteRecord);
        Task<List<PermissionDto>> GetAllGroupPermissionList(string PermissionType);
    }

}
