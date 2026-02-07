using Azure.Core;
using HRMS_API.NotificationService.HubService;
using HRMS_API.NotificationService.ManageService;
using HRMS_Core.Employee;
using HRMS_Core.Notifications;
using HRMS_Core.VM;
using HRMS_Core.VM.Employee;
using HRMS_Core.VM.ExitApplication;
using HRMS_Core.VM.Salary;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HRMS_API.Controllers.ExitApplicationAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExitApplicationAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;
        public ExitApplicationAPIController(IUnitOfWork unitOfWork, IHubContext<NotificationRemainderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }
        [HttpPost("CreateExitAppication")]
        public async Task<APIResponse> CreateExitAppication([FromBody] ExitApplicationVm model)
        {
            try
            {
                if (model == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Exit Application details cannot be null." };

                var result = await _unitOfWork.ExitApplicationRepository.CreateExitApplication(model);
                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = result.ResponseMessage };
                }

                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };

            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add record. Please try again later." };
            }
        }
        [HttpPut("UpdateExitApplication")]
        public async Task<APIResponse> UpdateExitApplication([FromBody] ExitApplicationVm employee)
        {
            try
            {
                if (employee == null)
                {
                    return new APIResponse() { isSuccess = false, ResponseMessage = "Employee details cannot be null" };
                }


                var result = await _unitOfWork.ExitApplicationRepository.UpdateExitApplication(employee);
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
                    ResponseMessage = "Unable to update record, Please try again later!"
                };
            }
        }

        [HttpDelete("Delete")]
        public async Task<APIResponse> Delete([FromBody] DeleteRecordVModel deleteRecord)
        {
            try
            {
                if (deleteRecord == null || deleteRecord.Id == null || !deleteRecord.Id.Any())
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No records selected for deletion."
                    };
                }

                var result = await _unitOfWork.ExitApplicationRepository.DeleteExitApplication(deleteRecord);
                if (result.Success != 1)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = result.ResponseMessage
                    };
                }

                await _unitOfWork.CommitAsync();

                return new APIResponse
                {
                    isSuccess = true,
                    Data = deleteRecord,
                    ResponseMessage = !string.IsNullOrEmpty(result.ResponseMessage)
                        ? result.ResponseMessage
                        : "Selected record(s) deleted successfully."
                };
            }
            catch (Exception err)
            {

                return new APIResponse
                {
                    isSuccess = false,
                    ResponseMessage = "An error occurred while processing your request. Please try again later."
                };
            }
        }


        [HttpGet("GetExitApplicationById")]
        public async Task<APIResponse> GetExitApplicationById(int EmployeeId)
        {
            try
            {
                var data = await _unitOfWork.ExitApplicationRepository.GetExitApplicationById(EmployeeId);
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

        [HttpPost("GetExitClearanceApproval")]
        public async Task<APIResponse> GetExitClearanceApproval(ExitApplicationFilterModel model)

        {
            try
            {
                var data = await _unitOfWork.ExitApplicationRepository.GetExitClearanceApproval(model);
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
        [HttpPost("GetExitApproval")]
        public async Task<APIResponse> GetExitApproval(ExitApprovalParam model)

        {
            try
            {
                var data = await _unitOfWork.ExitApplicationRepository.GetExitApproval(model);
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
        [HttpPut("UpdateExitApproval")]
        public async Task<APIResponse> UpdateExitApproval(List<ExitApplicationUpdateparam> requests)
        {
            try
            {
                // Check if requests are null or empty
                if (requests == null || requests.Count == 0)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No exit applications selected for update."
                    };
                }

                // Process each request in the list
                foreach (var request in requests)
                {
                    var result = await _unitOfWork.ExitApplicationRepository.UpdateExitApproval(request);
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
                    var applicationDetails = await _unitOfWork.ExitApplicationRepository.GetExitApplicationById(request.EmployeeId);
                    if (applicationDetails == null)
                    {
                        return new APIResponse
                        {
                            isSuccess = false,
                            ResponseMessage = $"Exit application with ID {request.EmployeeId} not found."
                        };
                    }

                    // Fetch reporting manager details
                    var reportingDetails = await _unitOfWork.EmployeeManageRepository.GetEmployeeById(request.EmployeeId);
                    if (reportingDetails == null)
                    {
                        return new APIResponse
                        {
                            isSuccess = false,
                            ResponseMessage = $"Reporting manager details not found for Employee ID {request.EmployeeId}."
                        };
                    }

                    // Convert boolean to proper status text
                    string statusText = request.IsApproved ? "approved" : "rejected";

                    var notification = new NotificationRemainders()
                    {
                        NotificationMessage = $"{reportingDetails.FullName} has {statusText} your exit application (Application ID: {request.ExitApplicationID}).",
                        NotificationTime = DateTime.UtcNow,
                        SenderId = reportingDetails.Id.ToString(),
                        ReceiverIds = applicationDetails.Employeeid.ToString(),
                        NotificationType = NotificationType.ExitApproval,
                        NotificationAffectedId = request.ExitApplicationID
                    };

                    var savedNotification = await _unitOfWork.NotificationRemainderRepository.CreateNotificationRemainder(notification);
                    if (savedNotification.Success <= 0)
                    {
                        return new APIResponse
                        {
                            isSuccess = false,
                            ResponseMessage = "Failed to create notification."
                        };
                    }

                    // Send real-time notification if employee is online
                    var employeeConnection = NotificationRemainderConnectionManager.GetConnections(applicationDetails.Employeeid.ToString());
                    if (employeeConnection.Any())
                    {
                        await _hubContext.Clients.Clients(employeeConnection)
                            .SendAsync("ReceiveNotificationRemainder", notification);
                    }
                }

                return new APIResponse
                {
                    isSuccess = true,
                    ResponseMessage = $"{requests.Count} exit application(s) updated successfully."
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
        [HttpPut("UpdateExitApprovalBYHR")]
        public async Task<APIResponse> UpdateExitApprovalBYHR(List<ExitApplicationUpdateparam> requests)
        {
            try
            {
                // Check if requests are null or empty
                if (requests == null || requests.Count == 0)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No exit applications selected for update."
                    };
                }

                // Process each request in the list
                foreach (var request in requests)
                {
                    var result = await _unitOfWork.ExitApplicationRepository.UpdateExitApprovalBYHR(request);
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
                    var applicationDetails = await _unitOfWork.ExitApplicationRepository.GetExitApplicationById(request.EmployeeId);
                    if (applicationDetails == null)
                    {
                        return new APIResponse
                        {
                            isSuccess = false,
                            ResponseMessage = $"Exit application with ID {request.EmployeeId} not found."
                        };
                    }

                    // Fetch reporting manager details
                    var reportingDetails = await _unitOfWork.EmployeeManageRepository.GetEmployeeById(request.EmployeeId);
                    if (reportingDetails == null)
                    {
                        return new APIResponse
                        {
                            isSuccess = false,
                            ResponseMessage = $"Reporting manager details not found for Employee ID {request.EmployeeId}."
                        };
                    }

                    // Convert boolean to proper status text
                    string statusText = request.IsApproved ? "approved" : "rejected";

                    var notification = new NotificationRemainders()
                    {
                        NotificationMessage = $"{reportingDetails.FullName} has {statusText} your exit application (Application ID: {request.ExitApplicationID}).",
                        NotificationTime = DateTime.UtcNow,
                        SenderId = reportingDetails.Id.ToString(),
                        ReceiverIds = applicationDetails.Employeeid.ToString(),
                        NotificationType = NotificationType.ExitApproval,
                        NotificationAffectedId = request.ExitApplicationID
                    };

                    var savedNotification = await _unitOfWork.NotificationRemainderRepository.CreateNotificationRemainder(notification);
                    if (savedNotification.Success <= 0)
                    {
                        return new APIResponse
                        {
                            isSuccess = false,
                            ResponseMessage = "Failed to create notification."
                        };
                    }

                    // Send real-time notification if employee is online
                    var employeeConnection = NotificationRemainderConnectionManager.GetConnections(applicationDetails.Employeeid.ToString());
                    if (employeeConnection.Any())
                    {
                        await _hubContext.Clients.Clients(employeeConnection)
                            .SendAsync("ReceiveNotificationRemainder", notification);
                    }
                }

                return new APIResponse
                {
                    isSuccess = true,
                    ResponseMessage = $"{requests.Count} exit application(s) updated successfully."
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


        [HttpPut("UpdateNOCFormFlag")]
        public async Task<APIResponse> UpdateNOCFormFlag(int exitApplicationId)
        {
            try
            {
                if (exitApplicationId <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid Exit Application ID." };

                var result = await _unitOfWork.ExitApplicationRepository.UpdateNOCFormFlag(exitApplicationId);

                if (result.Success > 0)
                {
                    return new APIResponse { isSuccess = true, ResponseMessage = "NOC Form flag updated successfully." };
                }
                return new APIResponse { isSuccess = false, ResponseMessage = result.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Unable to update NOC form flag. Please try again later."
                };
            }
        }

    }
}
