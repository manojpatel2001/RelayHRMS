using HRMS_Core.VM;
using HRMS_Core.VM.ApprovalManagement;
using HRMS_Core.VM.OtherMaster;
using HRMS_Core.VM.Probations;
using HRMS_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.OtherMaster
{
    public interface IManpowerRelationRepository
    {
        Task<APIResponse> CreateManpowerRelation(ManpowerRelationModel model);
        Task<APIResponse> UpdateManpowerRelation(ManpowerRelationModel model);
        Task<APIResponse> DeleteManpowerRelation(DeleteRecordVM delete);
        Task<APIResponse> GetAllManpowerRelation(int manpowerRequisitionId);
        Task<APIResponse> GetPendingManpowerApprovalRequestsWithHistory(ManpowerApprovalRequestFilter parameters);
        Task<APIResponse> GetAllManPowerStatus();

    }

}
