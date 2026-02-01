using HRMS_Core.VM;
using HRMS_Core.VM.OtherMaster;
using HRMS_Utility;

namespace HRMS_Infrastructure.Interface.OtherMaster
{
    public interface IManpowerAttachmentRepository
    {
        Task<APIResponse> CreateManpowerAttachment(ManpowerAttachmentModel model);
        Task<APIResponse> UpdateManpowerAttachment(ManpowerAttachmentModel model);
        Task<APIResponse> DeleteManpowerAttachment(DeleteRecordVM delete);
        Task<APIResponse> GetAllManpowerAttachment(int manpowerRequisitionId);
        Task<vmManpowerAttachment?> GetByManpowerAttachmentId(int manpowerAttachmentId);
    }

}
