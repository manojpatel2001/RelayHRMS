using HRMS_Core.VM.ApprovalManagement;
using HRMS_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.ApprovalManagement
{
    public interface IApprovalMasterRepository
    {
        Task<APIResponse> ManageApprovalMaster(ApprovalMasterViewModel model);
        Task<APIResponse> GetAllApprovalMasters();
        Task<APIResponse> GetAllApprovalMasterList(int? approvalTypeId);
        Task<APIResponse> GetAllApprovalMasterType();
    }
}
