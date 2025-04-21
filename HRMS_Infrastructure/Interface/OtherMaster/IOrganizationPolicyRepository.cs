using HRMS_Core.Master.OtherMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.OtherMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.OtherMaster
{
    public interface  IOrganizationPolicyRepository:IRepository<OrganizationPolicy>
    {
        Task<VMCommonResult> CreateOrganizationPolicy(vmOrganizationPolicy policy);
        Task<VMCommonResult> UpdateOrganizationPolicy(vmOrganizationPolicy policy);
        Task<VMCommonResult> DeleteOrganizationPolicy(DeleteRecordVM deleteRecordVM);

    }
}
