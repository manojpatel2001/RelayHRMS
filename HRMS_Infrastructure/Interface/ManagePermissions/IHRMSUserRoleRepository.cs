using HRMS_Core.DbContext;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.ManagePermissions
{
    public interface IHRMSUserRoleRepository:IRepository<HRMSUserRole>
    {
        Task<VMCommonResult> CreateUserRole(HRMSUserRole role);
        Task<VMCommonResult> UpdateUserRole(HRMSUserRole role);
        Task<VMCommonResult> DeleteUserRole(DeleteRecordVM record);
    }

}
