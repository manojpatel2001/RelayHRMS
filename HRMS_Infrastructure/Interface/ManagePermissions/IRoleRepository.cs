using HRMS_Core.DbContext;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.ManagePermissions
{
    public interface IRoleRepository:IRepository<HRMSRoleIdentity>
    {
        Task<List<HRMSRoleIdentity>> GetAllRoles();
        Task<VMCommonResult> CreateRole(HRMSRoleIdentity role);
        Task<VMCommonResult> UpdateRole(HRMSRoleIdentity role);
        Task<VMCommonResult> DeleteRole(DeleteRecordVM record);
    }
}
