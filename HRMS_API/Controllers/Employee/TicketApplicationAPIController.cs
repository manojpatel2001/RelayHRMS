using HRMS_API.Services;
using HRMS_Core.Employee;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Azure.Core.HttpHeader;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketApplicationAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileUploadService _fileUploadService;
        public TicketApplicationAPIController(IUnitOfWork unitOfWork, FileUploadService fileUploadService)
        {
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
        }

        [HttpPost("GetAllTicketApplications")]
        public async Task<APIResponse> GetAllTicketApplications(CommonParameter common)
        {
            try
            {
                var data = await _unitOfWork.TicketApplicationRepository.GetAllTicketApplications(common);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetTicketApplicationById/{ticketApplicationId}")]
        public async Task<APIResponse> GetTicketApplicationById(int ticketApplicationId)
        {
            try
            {
                var data = await _unitOfWork.TicketApplicationRepository.GetTicketApplicationById(ticketApplicationId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("CreateTicketApplication")]
        public async Task<APIResponse> CreateTicketApplication( vmTicketApplication model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Ticket application details cannot be null." };

                if (model.AttachDocumentFile != null)
                {
                    if (model.AttachDocumentFile.Length > 0)
                    {

                        var folder = $"uploads/ticket-attachment";
                        var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync(model.AttachDocumentFile, folder, null);
                        if (string.IsNullOrEmpty(fileUrl))
                        {
                            return new APIResponse { isSuccess = false, ResponseMessage = "Some thing went wrong. Please try again later." };

                        }
                        model.AttachDocumentUrl = fileUrl;
                    }
                    else
                    {
                        model.AttachDocumentUrl = null;
                    }
                }
                var ticketStatus = await _unitOfWork.TicketStatusRepository.GetTicketStatusByName("Open");
                model.TicketStatusId = ticketStatus?.TicketStatusId;

                var result = await _unitOfWork.TicketApplicationRepository.CreateTicketApplication(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateTicketApplication")]
        public async Task<APIResponse> UpdateTicketApplication( vmTicketApplication model)
        {
            try
            {
                if (model == null || model.TicketApplicationId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Ticket application details cannot be null." };

                var check = await _unitOfWork.TicketApplicationRepository.GetTicketApplicationById((int)model.TicketApplicationId);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid  record." };

                if (model.AttachDocumentFile != null)
                {
                    if (model.AttachDocumentFile.Length > 0)
                    {

                        var folder = $"uploads/ticket-attachment";
                        var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync(model.AttachDocumentFile, folder, check.AttachDocumentUrl);
                        if (string.IsNullOrEmpty(fileUrl))
                        {
                            return new APIResponse { isSuccess = false, ResponseMessage = "Some thing went wrong. Please try again later." };

                        }
                        model.AttachDocumentUrl = fileUrl;
                    }
                    else
                    {
                        model.AttachDocumentUrl = check.AttachDocumentUrl;
                    }
                   
                }
                else
                {
                    model.AttachDocumentUrl = check.AttachDocumentUrl;
                }
                var result = await _unitOfWork.TicketApplicationRepository.UpdateTicketApplication(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteTicketApplication")]
        public async Task<APIResponse> DeleteTicketApplication([FromBody] DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var result = await _unitOfWork.TicketApplicationRepository.DeleteTicketApplication(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

        [HttpPost("GetEmployeeAndTicketTypeByDepartmentId")]
        public async Task<APIResponse> GetEmployeeAndTicketTypeByDepartmentId(CommonParameter commonParameter)
        {
            try
            {
                var data = await _unitOfWork.TicketApplicationRepository.GetEmployeeAndTicketTypeByDepartmentId(commonParameter);
                var newData = new
                {
                    AssignList = data.Employees,
                    TicketTypes = data.TicketTypes
                };
                return new APIResponse { isSuccess = true, Data = newData, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpPost("GetAllAssignTicketApplications")]
        public async Task<APIResponse> GetAllAssignTicketApplications(CommonParameter common)
        {
            try
            {
                var data = await _unitOfWork.TicketApplicationRepository.GetAllAssignTicketApplications(common);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };
                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }
    }
}
