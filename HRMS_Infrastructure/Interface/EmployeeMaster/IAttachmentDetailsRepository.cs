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
        Task<SP_Response> CreateAttachmentDetail(VmAttachmentDetails model);
        Task<SP_Response> UpdateAttachmentDetail(VmAttachmentDetails model);
        Task<SP_Response> DeleteAttachmentDetail(DeleteRecordVM deleteRecord);
        Task<AttachmentDetails?> GetAttachmentDetailById(vmCommonGetById vmCommonGetById);
        Task<List<AttachmentDetails>> GetAllAttachmentDetails(vmCommonGetById vmCommonGetById);
        Task<List<DocumentType>> GetAllDocumentTypes();
    }
}
