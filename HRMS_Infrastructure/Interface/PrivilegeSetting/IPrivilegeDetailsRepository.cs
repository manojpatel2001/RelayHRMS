using HRMS_Core.PrivilegeSetting;
using HRMS_Core.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.PrivilegeSetting
{
    public interface IPrivilegeDetailsRepository:IRepository<PrivilegeDetails>
    {
        Task<VMCommonResult> CreatePrivilegeDetails(PrivilegeDetails privilegeDetails);
        Task<VMCommonResult> UpdatePrivilegeDetails(PrivilegeDetails privilegeDetails);
        Task<VMCommonResult> DeletePrivilegeDetails(DeleteRecordVM deleteRecord);

        Task<PrivilegeDetails?> GetPrivilegeDetailsById(int privilegeDetailsId);
    }
}
