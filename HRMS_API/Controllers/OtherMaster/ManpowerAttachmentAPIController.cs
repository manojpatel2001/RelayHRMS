using HRMS_API.Services;
using HRMS_Core.VM;
using HRMS_Core.VM.OtherMaster;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.OtherMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManpowerAttachmentAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileUploadService _fileUploadService;


        public ManpowerAttachmentAPIController(IUnitOfWork unitOfWork, FileUploadService  fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;

        }

        [HttpPost("Create")]
        public async Task<APIResponse> Create(ManpowerAttachmentModel model)
        {
            try
            {
                if (model.DocumentFile!=null)
                {
                    if (model.DocumentFile.Length > 0)
                    {

                        var folder = $"uploads/employeeprofile";
                        var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync(model.DocumentFile, folder, model.DocumentUrl);
                        if (string.IsNullOrEmpty(fileUrl))
                        {
                            return new APIResponse { isSuccess = false, ResponseMessage = "Some thing went wrong. Please try again later." };

                        }
                        model.DocumentUrl = fileUrl;
                    }
                    
                }
                var data = await _unitOfWork.ManpowerAttachmentRepository.CreateManpowerAttachment(model);
                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create attachment. Please try again later." };
            }
        }

        [HttpPost("Update")]
        public async Task<APIResponse> Update(ManpowerAttachmentModel model)
        {
            try
            {
                if (model.ManpowerAttachmentId == 0| model.ManpowerAttachmentId == null) 
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select valid record!" };
                }

                var details = await _unitOfWork.ManpowerAttachmentRepository.GetByManpowerAttachmentId((int)model.ManpowerAttachmentId);
                if (details!=null)
                {
                   model.DocumentUrl=details.DocumentUrl;
                }
                if (model.DocumentFile != null)
                {
                    if (model.DocumentFile.Length > 0)
                    {

                        var folder = $"uploads/manpower-attachment";
                        var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync(model.DocumentFile, folder, model.DocumentUrl);
                        if (string.IsNullOrEmpty(fileUrl))
                        {
                            return new APIResponse { isSuccess = false, ResponseMessage = "Some thing went wrong. Please try again later." };

                        }
                        model.DocumentUrl = fileUrl;
                    }

                }
                var data = await _unitOfWork.ManpowerAttachmentRepository.UpdateManpowerAttachment(model);
                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update attachment. Please try again later." };
            }
        }

        [HttpDelete("Delete")]
        public async Task<APIResponse> Delete(DeleteRecordVM delete)
        {
            try
            {
                var data = await _unitOfWork.ManpowerAttachmentRepository.DeleteManpowerAttachment( delete);
                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete attachment. Please try again later." };
            }
        }

        [HttpGet("GetAll/{manpowerRequisitionId}")]
        public async Task<APIResponse> GetAll(int manpowerRequisitionId)
        {
            try
            {
                var data = await _unitOfWork.ManpowerAttachmentRepository.GetAllManpowerAttachment(manpowerRequisitionId);
                return data;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve attachments. Please try again later." };
            }
        }
    }

}
