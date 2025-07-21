using HRMS_Core.EmployeeMaster;
using HRMS_Core.Helper;
using HRMS_Core.VM;
using HRMS_Core.VM.EmployeeMaster;
using HRMS_Core.VM.ManagePermision;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMS_API.Controllers.EmployeeMaster
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentDetailsAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;


        public AttachmentDetailsAPIController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpGet("GetAllAttachmentDetails/{EmployeeId}")]
        public async Task<APIResponse> GetAllAttachmentDetails(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.AttachmentDetailsRepository.GetAllAttachmentDetails(new vmCommonGetById { Id = EmployeeId });
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while retrieving records." };
            }
        }

        [HttpGet("GetAttachmentDetailById/{id}")]
        public async Task<APIResponse> GetAttachmentDetailById(int id)
        {
            try
            {
                var data = await _unitOfWork.AttachmentDetailsRepository.GetAttachmentDetailById(new vmCommonGetById { Id = id });
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while retrieving record." };
            }
        }

        [HttpPost("CreateAttachmentDetail")]
        public async Task<APIResponse> CreateAttachmentDetail(VmAttachmentDetails model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Attachment details cannot be null." };

                if(model.AttachmentFile == null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Attachment file cannot be null." };
                }
                if (model.AttachmentFile.Length <= 0)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Attachment file cannot be null." };
                }
                var baseUrl = _configuration["BaseUrlSettings:BaseUrl"];
                if (string.IsNullOrEmpty(baseUrl))
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Some thing went wrong. Please trye again" };

                }
                var folder = $"uploads/employeeattachment";
                var fileUrl = await UploadDocument.UploadAndReplaceDocumentAsync(baseUrl, model.AttachmentFile, folder, null);
                if (!string.IsNullOrEmpty(fileUrl))
                {
                    model.DocumentUrl = fileUrl;
                }
                else
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to upload document" };

                }

                var result = await _unitOfWork.AttachmentDetailsRepository.CreateAttachmentDetail(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "Record created successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create record." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while creating record." };
            }
        }

        [HttpPut("UpdateAttachmentDetail")]
        public async Task<APIResponse> UpdateAttachmentDetail(VmAttachmentDetails model)
        {
            try
            {
                if (model == null || model.AttachmentDetailsId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid attachment details." };

                var existing = await _unitOfWork.AttachmentDetailsRepository.GetAttachmentDetailById(new vmCommonGetById { Id = model.AttachmentDetailsId });
                if (existing == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                var baseUrl = _configuration["BaseUrlSettings:BaseUrl"];
                if (string.IsNullOrEmpty(baseUrl))
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Some thing went wrong. Please trye again" };

                }
                if (model.AttachmentFile != null)
                {
                    if (model.AttachmentFile.Length > 0)
                    {
                        var folder = $"uploads/employeeattachment";
                        var fileUrl = await UploadDocument.UploadAndReplaceDocumentAsync(baseUrl, model.AttachmentFile, folder, existing.DocumentUrl);
                        if (!string.IsNullOrEmpty(fileUrl))
                        {
                            model.DocumentUrl = fileUrl;
                        }
                        else
                        {
                            return new APIResponse { isSuccess = false, ResponseMessage = "Unable to upload document" };
                        }
                    }
                }
                else
                {
                    model.DocumentUrl= existing.DocumentUrl;
                }


                    var result = await _unitOfWork.AttachmentDetailsRepository.UpdateAttachmentDetail(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "Record updated successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while updating record." };
            }
        }

        [HttpDelete("DeleteAttachmentDetail")]
        public async Task<APIResponse> DeleteAttachmentDetail([FromBody] DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid delete request." };

                var existing = await _unitOfWork.AttachmentDetailsRepository.GetAttachmentDetailById(new vmCommonGetById { Id = model.Id });
                if (existing == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                var result = await _unitOfWork.AttachmentDetailsRepository.DeleteAttachmentDetail(model);

                if (result.Id > 0)
                    return new APIResponse { isSuccess = true, ResponseMessage = "Record deleted successfully." };

                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Error while deleting record." };
            }
        }
    }
}
