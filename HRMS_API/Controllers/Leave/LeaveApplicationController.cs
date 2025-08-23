
using HRMS_API.NotificationService.HubService;
using HRMS_API.NotificationService.ManageService;
using HRMS_Core.Leave;
using HRMS_Core.Notifications;
using HRMS_Core.VM.Leave;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net.Sockets;

namespace HRMS_API.Controllers.Leave
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeaveApplicationController : ControllerBase
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;

        public LeaveApplicationController(IUnitOfWork unitOfWork, IHubContext<NotificationRemainderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        [HttpPost("AddLeaveapplication")]

        public async Task<APIResponse> AddLeaveapplication([FromBody]LeaveApplication Leave)
        {
            try
            {
                Leave.CreatedDate = DateTime.Now;
                Leave.LeaveStatus = "Pending";

                var isexist = await _unitOfWork.LeaveApplicationRepository.GetAsync(asp => asp.EmplooyeId == Leave.EmplooyeId && asp.FromDate == Leave.FromDate && asp.Todate == Leave.Todate);
             
                if (isexist != null)
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Alredy leave applyed for this period" };


                }
                var isexistfromdate = await _unitOfWork.LeaveApplicationRepository.GetAsync(asp => asp.EmplooyeId == Leave.EmplooyeId && asp.FromDate == Leave.FromDate && asp.Todate != Leave.Todate);

                if (isexistfromdate != null)
                {
                    var issoftdelete = await _unitOfWork.LeaveApplicationRepository.softdelete(Leave);

                }


                var isSaved = await _unitOfWork.LeaveApplicationRepository.InsertLeaveApplicationAsync(Leave);

                if (isSaved.Success<=0)
                    return new APIResponse { isSuccess = false, ResponseMessage =isSaved.ResponseMessage };

                //Notification send to reporting persion
                var employeeDetails = await _unitOfWork.EmployeeManageRepository.GetEmployeeById((int)Leave.EmplooyeId);
                var notification = new NotificationRemainders()
                {
                    NotificationMessage = $"{employeeDetails?.FullName} has applied for leave from {Leave.FromDate:dd-MM-yyyy} to {Leave.Todate:dd-MM-yyyy}. Awaiting your approval.",
                    NotificationTime = DateTime.UtcNow,
                    SenderId = Leave.EmplooyeId.ToString(),
                    ReceiverIds=Leave.ReportingManagerId.ToString(),
                    NotificationType= NotificationType.LeaveApplication,
                    NotificationAffectedId=isSaved.Success
                };
                var savedNotification = await _unitOfWork.NotificationRemainderRepository.CreateNotificationRemainder(notification);
                if (savedNotification.Success > 0)
                {
                    notification.NotificationRemainderId = savedNotification.Success;
                    var reprtingConnection = NotificationRemainderConnectionManager.GetConnections(Leave.ReportingManagerId.ToString());
                    if (reprtingConnection.Any())
                    {
                        await _hubContext.Clients.Clients(reprtingConnection).SendAsync("ReceiveNotificationRemainder", notification);
                    }
                }


                return new APIResponse { isSuccess = true, ResponseMessage = isSaved.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to add records. Please try again later." };
            }


        }
        [HttpPost("GetReportingperson")]
        public async Task<APIResponse> GetReportingperson(int Empid)
        {
            try
            {
                var employee = await _unitOfWork.EmployeeManageRepository.GetAsync(e => e.Id == Empid);
                if (employee == null || employee.ReportingManagerId == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Employee or Reporting Manager not found."
                    };
                }

                var manager = await _unitOfWork.EmployeeManageRepository.GetAsync(m => m.Id == employee.ReportingManagerId);
                if (manager == null)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "Reporting Manager not found."
                    };
                }

                var result = new
                {
                    Id = manager.Id,
                    FullName = manager.FullName
                };

                return new APIResponse
                {
                    isSuccess = true,
                    Data = result,
                    ResponseMessage = "Reporting manager fetched successfully."
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Error occurred while fetching reporting person."
                };
            }
        }


        [HttpPost("GetLeaveApplications")]
        public async Task<APIResponse> GetLeaveApplications([FromBody] SearchVmCompOff filter)
        {
            try
            {
                var result = await _unitOfWork.LeaveApplicationRepository.GetLeaveApplicationsAsync(filter);
                if (result == null)
                {
                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = "Data Fetched not Sucessfully"
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = result,
                    ResponseMessage = "Data Fetched Sucessfully"
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = true,
                    ResponseMessage = "Data not fetched successfully."
                };
            }
        }



        [HttpPost("GetLeaveApplicationsforApprove")]
        public async Task<APIResponse> GetLeaveApplicationsforApprove([FromBody] SearchVmCompOff filter)
        {
            try
            {
                var result = await _unitOfWork.LeaveApplicationRepository.GetLeaveApplicationsforApprove(filter);
                if (result == null)
                {
                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = "Data Fetched not Sucessfully"
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = result,
                    ResponseMessage = "Data Fetched Sucessfully"
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = true,
                    ResponseMessage = "Data not fetched successfully."
                };
            }
        }


        [HttpPost("LeaveapplicationApproveorReject")]
        public async Task<APIResponse> LeaveapplicationApproveorReject(LeaveaprovalVM LVM)
        {
            try
            {
                LVM.Date = DateTime.UtcNow;

                if (LVM.Ids == null || !LVM.Ids.Any() || string.IsNullOrEmpty(LVM.Status))
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid input." };
                }
                

                var isSaved = await _unitOfWork.LeaveApplicationRepository.Updateapproval(LVM);

                if (isSaved.Success<1)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Failed to update Comp Off details." };

                if (LVM.Status == "Approved")
                {
                    var leavemanage = await _unitOfWork.CompOffDetailsRepository.UpdateLeavedetails(LVM.Ids, LVM.Status);
                    if (!leavemanage)
                        return new APIResponse
                        { isSuccess = false, ResponseMessage = "Failed to update leave details." };

                }

                //Notification send to employee persion
                foreach (var applicationId in LVM.Ids)
                {
                    var applicationDetails = await _unitOfWork.LeaveApplicationRepository.GetLeaveApplicationById(applicationId);
                    if (applicationDetails != null)
                    {
                        var reportingDetails = await _unitOfWork.EmployeeManageRepository.GetEmployeeById((int)LVM.EmployeeId);

                        var notification = new NotificationRemainders()
                        {
                            NotificationMessage = $"{reportingDetails?.FullName} has {LVM.Status} your leave from {applicationDetails.FromDate:dd-MM-yyyy} to {applicationDetails.Todate:dd-MM-yyyy} ",
                            NotificationTime = DateTime.UtcNow,
                            SenderId = reportingDetails?.Id.ToString(),
                            ReceiverIds =  applicationDetails.EmplooyeId.ToString(),
                            NotificationType = NotificationType.LeaveApproval,
                            NotificationAffectedId = applicationId
                        };
                        var savedNotification = await _unitOfWork.NotificationRemainderRepository.CreateNotificationRemainder(notification);
                        if (savedNotification.Success > 0)
                        {
                            notification.NotificationRemainderId = savedNotification.Success;
                            var employeeConnection = NotificationRemainderConnectionManager.GetConnections(applicationDetails.EmplooyeId.ToString());
                            if (employeeConnection.Any())
                            {
                                await _hubContext.Clients.Clients(employeeConnection).SendAsync("ReceiveNotificationRemainder", notification);
                            }
                        }
                    }
                }
                return new APIResponse { isSuccess = true, ResponseMessage = isSaved.ResponseMessage };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = ex.Message,
                    ResponseMessage = "Unable to update records. Please try again later."
                };
            }
        }


        [HttpPost("GetLeaveApplicationsforApproveAdmin")]
        public async Task<APIResponse> GetLeaveApplicationsforApproveAdmin([FromBody] SearchVmCompOff filter)
        {
            try
            {
                var result = await _unitOfWork.LeaveApplicationRepository.GetLeaveApplicationsforApproveAdmin(filter);
                if (result == null)
                {
                    return new APIResponse
                    {
                        isSuccess = true,
                        ResponseMessage = "Data Fetched not Sucessfully"
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = result,
                    ResponseMessage = "Data Fetched Sucessfully"
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    isSuccess = true,
                    ResponseMessage = "Data not fetched successfully."
                };
            }
        }


        [HttpPost("GetLeaveApproval")]
        public async Task<APIResponse> GetLeaveApproval([FromBody] LeaveApp_Param vm)
        {
            try
            {
                var data = await _unitOfWork.LeaveApplicationRepository.GetLeaveApproval(vm);

                if (data == null || data.Count == 0)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No leave records found."
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Leave records fetched successfully."
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrieve leave records. Please try again later!"
                };
            }
        }

        [HttpPost("GetLeaveBalance")]
        public async Task<APIResponse> GetLeaveBalance([FromBody] LeaveApp_Param vm)
        {
            try
            {
                var data = await _unitOfWork.LeaveApplicationRepository.GetLeaveBalance(vm);

                if (data == null || data.Count == 0)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No leave records found."
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Leave records fetched successfully."
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrieve leave records. Please try again later!"
                };
            }
        }

        [HttpPost("GetLeaveType")]
        public async Task<APIResponse> GetLeaveType([FromBody] LeaveDetailsvm vm)
        {
            try
            {
                var data = await _unitOfWork.LeaveApplicationRepository.GetLeaveDetails(vm);

                if (data == null || data.Count == 0)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No leave records found."
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Leave records fetched successfully."
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrieve leave records. Please try again later!"
                };
            }
        }

        [HttpGet("GetActiveLeaveDetails")]
        public async Task<APIResponse> GetActiveLeaveDetails()
        {
            try
            {
                var data = await _unitOfWork.LeaveApplicationRepository.GetActiveLeaveDetails();

                if (data == null || data.Count == 0)
                {
                    return new APIResponse
                    {
                        isSuccess = false,
                        ResponseMessage = "No leave records found."
                    };
                }

                return new APIResponse
                {
                    isSuccess = true,
                    Data = data,
                    ResponseMessage = "Leave records fetched successfully."
                };
            }
            catch (Exception err)
            {
                return new APIResponse
                {
                    isSuccess = false,
                    Data = err.Message,
                    ResponseMessage = "Unable to retrieve leave records. Please try again later!"
                };
            }
        }

        [HttpGet("GetLeaveApplicationById/{leaveApplicationId}")]
        public async Task<APIResponse> GetLeaveApplicationById(int leaveApplicationId)
        {
            try
            {
                var data = await _unitOfWork.LeaveApplicationRepository.GetLeaveApplicationById(leaveApplicationId);
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
