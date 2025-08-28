using HRMS_API.NotificationService.HubService;
using HRMS_API.NotificationService.ManageService;
using HRMS_API.Services;
using HRMS_Core.Employee;
using HRMS_Core.Notifications;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketFollowUpAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileUploadService _fileUploadService;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;

        public TicketFollowUpAPIController(IUnitOfWork unitOfWork, FileUploadService fileUploadService, IHubContext<NotificationRemainderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
            _hubContext = hubContext;
        }

        [HttpGet("GetAllTicketFollowUpByApplicationId/{ticketApplicationId}")]
        public async Task<APIResponse> GetAllTicketFollowUpByApplicationId(int ticketApplicationId)
        {
            try
            {
                var data = await _unitOfWork.TicketFollowUpRepository.GetAllTicketFollowUpByApplicationId(ticketApplicationId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpPost("CreateTicketFollowUp")]
        public async Task<APIResponse> CreateTicketFollowUp(vmTicketFollowUp model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Ticket follow-up details cannot be null." };

                // Handle file upload if needed
                if (model.FollowUpDocumentFile != null && model.FollowUpDocumentFile.Length > 0)
                {
                    var folder = $"uploads/ticket-followup-attachment";
                    var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync(model.FollowUpDocumentFile, folder, null);
                    if (string.IsNullOrEmpty(fileUrl))
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = "Something went wrong while uploading the file. Please try again later." };
                    }
                    model.FollowUpDocumentUrl = fileUrl;
                }

                var result = await _unitOfWork.TicketFollowUpRepository.CreateTicketFollowUp(model);
                if (result.Success <= 0)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };

                }


                //Notification send to reporting persion
                var employeeDetails = await _unitOfWork.EmployeeManageRepository.GetEmployeeById((int)model.EmployeeId);
                if (employeeDetails != null)
                {
                    var ticketApplication = await _unitOfWork.TicketApplicationRepository.GetTicketApplicationById((int)model.TicketApplicationId);
                    if (ticketApplication != null)
                    {
                        var notificationType = ticketApplication.TicketAssignId == model?.EmployeeId ? NotificationType.TicketResponse : NotificationType.TicketFollowUp;
                        var ReceiverIds = ticketApplication.TicketAssignId == model?.EmployeeId ? ticketApplication.EmployeeId.ToString() : ticketApplication.TicketAssignId.ToString();
                        var notification = new NotificationRemainders()
                        {
                            NotificationMessage = $"{employeeDetails?.FullName} has added new follow on ticket. Please check and give response.",
                            NotificationTime = DateTime.UtcNow,
                            SenderId = model?.EmployeeId.ToString(),
                            ReceiverIds = ReceiverIds,
                            NotificationType = notificationType,
                            NotificationAffectedId = result.Success
                        };
                        var savedNotification = await _unitOfWork.NotificationRemainderRepository.CreateNotificationRemainder(notification);
                        if (savedNotification.Success > 0)
                        {
                            notification.NotificationRemainderId = savedNotification.Success;
                            var recieveConnection = NotificationRemainderConnectionManager.GetConnections(ReceiverIds.ToString());
                            if (recieveConnection.Any())
                            {
                                await _hubContext.Clients.Clients(recieveConnection).SendAsync("ReceiveNotificationRemainder", notification);
                            }
                        }
                    }
                }

                return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };


            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }

        [HttpPut("UpdateTicketFollowUp")]
        public async Task<APIResponse> UpdateTicketFollowUp(vmTicketFollowUp model)
        {
            try
            {
                if (model == null || model.TicketFollowUpId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Ticket follow-up details cannot be null." };

                var check = await _unitOfWork.TicketFollowUpRepository.GetTicketFollowUpById((int)model.TicketFollowUpId);
                if (check == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid record." };

                if (model.FollowUpDocumentFile != null)
                {
                    if (model.FollowUpDocumentFile.Length > 0)
                    {
                        var folder = $"uploads/ticket-followup-attachment";
                        var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync(model.FollowUpDocumentFile, folder, check.FollowUpDocumentUrl);
                        if (string.IsNullOrEmpty(fileUrl))
                        {
                            return new APIResponse { isSuccess = false, ResponseMessage = "Something went wrong while uploading the file. Please try again later." };
                        }
                        model.FollowUpDocumentUrl = fileUrl;
                    }
                    else
                    {
                        model.FollowUpDocumentUrl = check.FollowUpDocumentUrl;
                    }
                }
                else
                {
                    model.FollowUpDocumentUrl = check.FollowUpDocumentUrl;
                }

                var result = await _unitOfWork.TicketFollowUpRepository.UpdateTicketFollowUp(model);
                if (result.Success <= 0)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
                }
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update record. Please try again later." };
            }
        }

        [HttpDelete("DeleteTicketFollowUp")]
        public async Task<APIResponse> DeleteTicketFollowUp( DeleteRecordVM model)
        {
            try
            {
                if (model == null || model.Id == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Delete details cannot be null." };

                var result = await _unitOfWork.TicketFollowUpRepository.DeleteTicketFollowUp(model);
                if (result.Success <= 0)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
                }
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete record. Please try again later." };
            }
        }

        [HttpGet("GetTicketFollowUpById/{ticketFollowUpId}")]
        public async Task<APIResponse> GetTicketFollowUpById(int ticketFollowUpId)
        {
            try
            {
                var data = await _unitOfWork.TicketFollowUpRepository.GetTicketFollowUpById(ticketFollowUpId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }
    }
}
