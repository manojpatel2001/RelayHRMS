using HRMS_API.NotificationService.HubService;
using HRMS_API.NotificationService.ManageService;
using HRMS_Core.Notifications;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.Leave;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HRMS_API.Controllers.Leave
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveCancellationAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;

        public LeaveCancellationAPIController(IUnitOfWork unitOfWork, IHubContext<NotificationRemainderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        [HttpGet("GetById/{id}")]
        public async Task<APIResponse> GetById(int Id)
        {
            try
            {
                var data = await _unitOfWork.LeaveCancellationRepository.GetAsync(x => x.LeaveCancellationId == Id && x.IsEnabled == true && x.IsDeleted == false);
                if (data == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Record not found"
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Record fetched successfully"
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrive records, Please try again later!"
                };
            }

        }
        [HttpPost("GetEmpLeaveCancellationRequestReport")]
        public async Task<APIResponse> GetEmpLeaveCancellationRequestReport([FromBody] LeaveCancellationRequestFilterViewModel model)
        {
            try
            {
                var data = await _unitOfWork.LeaveCancellationRepository.GetEmpLeaveCancellationRequestReport(model);
                if (data == null)
                {
                    return new APIResponse() { isSuccess = true, ResponseMessage = "Record not fetched successfully" };

                }

                return new APIResponse() { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrieve records, Please try again later!"
                };
            }
        }
        [HttpPost("GetLeavecancellation")]
        public async Task<APIResponse> GetLeavecancellation([FromBody] LeaveCancellationReportRequest model)
        {
            try
            {
                var data = await _unitOfWork.LeaveCancellationRepository.GetLeavecancellationReport(model);
                if (data == null)
                {
                    return new APIResponse() { isSuccess = true, ResponseMessage = "Record not fetched successfully" };

                }

                return new APIResponse() { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrieve records, Please try again later!"
                };
            }
        }

        [HttpPost("CreateLeaveCancellation")]
        public async Task<APIResponse> CreateLeaveCancellation([FromBody] List<LeaveCancellationRequestVM> models)
        {
            try
            {
                if (models == null || !models.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records to save." };

                var firstModel = models.First();
                var savedIds = new List<int>();

                // Create all leave cancellation requests
                foreach (var model in models)
                {
                    var spResponse = await _unitOfWork.LeaveCancellationRepository.CreateLeavecancellation(model);
                    if (spResponse.Success <= 0)
                    {
                        return new APIResponse { isSuccess = false, ResponseMessage = spResponse.ResponseMessage };
                    }
                    savedIds.Add(spResponse.Success);
                }

                // Get leave application details first
               // Get employee details
                    var employeeDetails = await _unitOfWork.EmployeeManageRepository.GetEmployeeById((int)firstModel.EmployeeId);

                    if (employeeDetails != null)
                    {
                        var notification = new NotificationRemainders()
                        {
                            NotificationMessage = $"{employeeDetails.FullName} has requested cancellation for leave from {firstModel.FromDate:dd-MM-yyyy} to {firstModel.ToDate:dd-MM-yyyy}.",
                            NotificationTime = DateTime.UtcNow,
                            SenderId = employeeDetails.Id.ToString(),
                            ReceiverIds = employeeDetails.ReportingManagerId.ToString(),
                            NotificationType = NotificationType.LeaveCancellation,
                            NotificationAffectedId = savedIds.First()
                        };

                        var savedNotification = await _unitOfWork.NotificationRemainderRepository.CreateNotificationRemainder(notification);
                        if (savedNotification.Success > 0)
                        {
                            notification.NotificationRemainderId = savedNotification.Success;
                            var reportingConnection = NotificationRemainderConnectionManager.GetConnections(employeeDetails.ReportingManagerId.ToString());
                            if (reportingConnection.Any())
                            {
                                await _hubContext.Clients.Clients(reportingConnection).SendAsync("ReceiveNotificationRemainder", notification);
                            }
                        }
                    }
                

                return new APIResponse { isSuccess = true, ResponseMessage = "Leave cancellation request submitted successfully!" };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add records. Please try again later." };
            }
        }
        [HttpPut("UpdateLeaveCancellationStatus")]
        public async Task<APIResponse> UpdateLeaveCancellationStatus(List<updateLeaveCancellationRequestVM> requests)
        {
            try
            {
                if (requests == null || requests.Count == 0)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records selected for update." };
                }
                // Process each request in the list
                foreach (var request in requests)
                {
                    var result = await _unitOfWork.LeaveCancellationRepository.UpdateLeavecancellation(request);
                    if (result.Success <= 0)
                    {
                        return new APIResponse
                        {
                            isSuccess = false,
                            ResponseMessage = result.ResponseMessage
                        };
                    }
                }
                // Send notifications for each request
                foreach (var request in requests)
                {
                    var applicationDetails = await _unitOfWork.LeaveCancellationRepository
                        .GetLeavecancellationById(request.LeaveCancellationId);

                    if (applicationDetails != null)
                    {
                        var reportingDetails = await _unitOfWork.EmployeeManageRepository
                            .GetEmployeeById(request.EmployeeId);

                        // Convert boolean to proper status text
                        string statusText = request.IsApproved ? "approved" : "rejected";

                        var notification = new NotificationRemainders()
                        {
                            NotificationMessage = $"{reportingDetails?.FullName} has {statusText} your leave cancellation request from {applicationDetails.FromDate:dd-MM-yyyy} to {applicationDetails.ToDate:dd-MM-yyyy}",
                            NotificationTime = DateTime.UtcNow,
                            SenderId = reportingDetails?.Id.ToString(),
                            ReceiverIds = applicationDetails.EmployeeId.ToString(),
                            NotificationType = NotificationType.LeaveCancellationApproval,
                            NotificationAffectedId = request.LeaveCancellationId
                        };

                        var savedNotification = await _unitOfWork.NotificationRemainderRepository
                            .CreateNotificationRemainder(notification);

                        if (savedNotification.Success > 0)
                        {
                            notification.NotificationRemainderId = savedNotification.Success;
                            var employeeConnection = NotificationRemainderConnectionManager
                                .GetConnections(applicationDetails.EmployeeId.ToString());

                            if (employeeConnection.Any())
                            {
                                await _hubContext.Clients.Clients(employeeConnection)
                                    .SendAsync("ReceiveNotificationRemainder", notification);
                            }
                        }
                    }
                }
                return new APIResponse
                {
                    isSuccess = true,
                    ResponseMessage = $"{requests.Count} leave cancellation request(s) updated successfully."
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to update records. Please try again later."
                };
            }
        }
        [HttpDelete("Delete")]
        public async Task<APIResponse> Delete(DeleteRecordVM DeleteRecord)
        {
            try
            {
                if (DeleteRecord == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Delete details cannot be null" };
                }

                var result = await _unitOfWork.LeaveCancellationRepository.DeleteLeavecancellation(DeleteRecord);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to delete records, Please try again later!"
                };
            }
        }

        [HttpPost("GetReportingWiseLeaveCancellationRequestReport")]
        public async Task<APIResponse> GetReportingWiseLeaveCancellationRequestReport([FromBody] vmLeaveCancellationReportFilter model)
        {
            try
            {
                var data = await _unitOfWork.LeaveCancellationRepository.GetReportingWiseLeaveCancellationRequestReport(model);
                if (data == null)
                {
                    return new APIResponse() { isSuccess = true, ResponseMessage = "Record not fetched successfully" };

                }

                return new APIResponse() { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully" };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrieve records, Please try again later!"
                };
            }
        }

    }
}
