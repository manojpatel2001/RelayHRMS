using HRMS_Core.EmployeeMaster;
using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Core.VM.ManagePermision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRMS_Infrastructure.Interface.EmployeeMaster
{
    public interface IAttachmentDetailsRepository
    {
        Task<VMCommonResult> CreateAttachmentDetail(VmAttachmentDetails model);
        Task<VMCommonResult> UpdateAttachmentDetail(VmAttachmentDetails model);
        Task<VMCommonResult> DeleteAttachmentDetail(DeleteRecordVM deleteRecord);
        Task<AttachmentDetails?> GetAttachmentDetailById(vmCommonGetById vmCommonGetById);
        Task<List<AttachmentDetails>> GetAllAttachmentDetails(vmCommonGetById vmCommonGetById);
    }
}
