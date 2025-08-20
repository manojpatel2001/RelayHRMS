using HRMS_API.NotificationService.HubService;
using HRMS_API.NotificationService.ManageService;
using HRMS_Core.Leave;
using HRMS_Core.Notifications;
using HRMS_Core.VM.Leave;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Repository;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HRMS_API.Controllers.Leave
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompOffAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;
        public CompOffAPIController(IUnitOfWork unitOfWork, IHubContext<NotificationRemainderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        [HttpPost("CompOffDetailsApplication")]

        public async Task<APIResponse> CompOffDetailsApplication([FromBody] Comp_Off_Details COA)
        {
            try
            {
                var empcode = await _unitOfWork.EmployeeManageRepository.GetAsync(asd => asd.Id == COA.Emp_Id && asd.CompanyId == COA.Cmp_Id);
                var comoffdata = new Comp_Off_Details
                {
                    CreatedDate = DateTime.Now,
                    CreatedBy= COA.CreatedBy,
                    Cmp_Id= COA.Cmp_Id,
                    Emp_Id= COA.Emp_Id,
                    Rep_Person_Id= COA.Rep_Person_Id,
                    ApplicationDate = DateTime.Now,
                    Extra_Work_Day= COA.Extra_Work_Day,
                    Extra_Work_Hours=COA.Extra_Work_Hours,
                    Application_Status="Pending",
                    ComoffReason=COA.ComoffReason,
                    Emp_Code = empcode?.EmployeeCode

                };
               
                var isSaved = await _unitOfWork.CompOffDetailsRepository.InsertCompOffAsync(comoffdata);

                if (isSaved.Success <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = isSaved.ResponseMessage };

                //Notification send to reporting persion
                var employeeDetails = await _unitOfWork.EmployeeManageRepository.GetEmployeeById((int)COA.Emp_Id);
                var notification = new NotificationRemainders()
                {
                    NotificationMessage = $"{employeeDetails?.FullName} has applied for CompOff for Extra Work On : {COA.Extra_Work_Day:dd-MM-yyyy}. Awaiting your approval.",
                    NotificationTime = DateTime.UtcNow,
                    SenderId = COA.Emp_Id.ToString(),
                    ReceiverIds = COA.Rep_Person_Id.ToString(),
                    NotificationType = NotificationType.CompOffApplication,
                    NotificationAffectedId = isSaved.Success
                };
                var savedNotification = await _unitOfWork.NotificationRemainderRepository.CreateNotificationRemainder(notification);
                if (savedNotification.Success > 0)
                {
                    notification.NotificationRemainderId = savedNotification.Success;
                    var reprtingConnection = NotificationRemainderConnectionManager.GetConnections(COA.Rep_Person_Id.ToString());
                    if (reprtingConnection.Any())
                    {
                        await _hubContext.Clients.Clients(reprtingConnection).SendAsync("ReceiveNotificationRemainder", notification);
                    }
                }

                return new APIResponse { isSuccess = true, ResponseMessage = "Records Added successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to add records. Please try again later." };
            }
        
        }



        [HttpPost("CompOffDetailsApproveorReject")]
        public async Task<APIResponse> CompOffDetailsApproveorReject(ApproveandrejectVM ARVM)
        {
            try
            {
                if (ARVM.CompoffIds == null || !ARVM.CompoffIds.Any() || string.IsNullOrEmpty(ARVM.Status))
                {
                    return new APIResponse { isSuccess = false, ResponseMessage = "Invalid input." };
                }

                var isSaved = await _unitOfWork.CompOffDetailsRepository.Updateapproval(ARVM.CompoffIds, ARVM.Status);

                if (!isSaved)
                    return new APIResponse 
                    { isSuccess = false, ResponseMessage = "Failed to update Comp Off details." };


                if(ARVM.Status== "Approved")
                {
                    var leavemanage = await _unitOfWork.CompOffDetailsRepository.UpdateLeaveManger(ARVM.CompoffIds, ARVM.Status);
                    if (!leavemanage)
                        return new APIResponse
                        { isSuccess = false, ResponseMessage = "Failed to update leave details." };
                }
               
                return new APIResponse { isSuccess = true, ResponseMessage = "Records updated successfully." };
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



        [HttpPost("ReportingPersonEmpVise")]
        public async Task<APIResponse> ReportingPersonEmpVise(int Compid,int Empid)
        {
            try
            {
                var data = await _unitOfWork.EmployeeManageRepository.GetAllAsync(asd => asd.IsEnabled == true && asd.IsDeleted == false && asd.IsBlocked==false && asd.CompanyId== Compid && asd.Id!= Empid);
                if (data == null)
                {
                    return new APIResponse() { isSuccess = true, ResponseMessage = "Record not fetched successfully" };

                }
                var newdata = data.Select(leave => new
                {
                    ReportingPersonId = leave.Id,
                    ReportingPersonName = leave.FullName
                });
                return new APIResponse() { isSuccess = true, Data = newdata, ResponseMessage = "Record fetched successfully" };
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


        [HttpPost("GetCompOffApplications")]
        public async Task<APIResponse> GetCompOffApplications([FromBody] SearchVmCompOff search)
        {
            try
            {
                var data = await _unitOfWork.CompOffDetailsRepository.GetCompOffApplicationsAsync(search);
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

        [HttpPost("GetCompOffApplicationsAdmin")]
        public async Task<APIResponse> GetCompOffApplicationsAdmin([FromBody] SearchVmCompOff search)
        {
            try
            {
                var data = await _unitOfWork.CompOffDetailsRepository.GetCompOffApplicationsAdmin(search);
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
