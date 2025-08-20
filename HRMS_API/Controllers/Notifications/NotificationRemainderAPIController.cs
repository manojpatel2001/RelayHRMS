using HRMS_API.NotificationService.HubService;
using HRMS_Core.VM.Notifications;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HRMS_API.Controllers.Notifications
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationRemainderAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public NotificationRemainderAPIController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet("GetAllNotificationByUserId/{userId}")]
        public async Task<APIResponse> GetAllNotificationByUserId(int userId)
        {
            try
            {
                var data = await _unitOfWork.NotificationRemainderRepository.GetAllNotificationByUserId(userId);
                if (data == null || !data.Any())
                    return new APIResponse { isSuccess = false, ResponseMessage = "No records found." };

                var newData = new
                {
                    Notifications= data,
                    NotificationCount=data.FirstOrDefault()?.NotificationCount
                };
                return new APIResponse { isSuccess = true, Data = newData, ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false,  ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpGet("GetNotificationRemainderById/{notificationRemainderId}")]
        public async Task<APIResponse> GetNotificationRemainderById(int notificationRemainderId)
        {
            try
            {
                var data = await _unitOfWork.NotificationRemainderRepository.GetNotificationRemainderById(notificationRemainderId);
                if (data == null)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Record not found." };

                return new APIResponse { isSuccess = true, Data = data, ResponseMessage = "Record fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false,ResponseMessage = "Unable to retrieve record. Please try again later." };
            }
        }

        [HttpPost("ReadNotificationRemainder")]
        public async Task<APIResponse> GetNotificationRemainderById(vmReadNotificationRemainder vmReadNotificationRemainder)
        {
            try
            {
                var data = await _unitOfWork.NotificationRemainderRepository.ReadNotificationRemainder(vmReadNotificationRemainder);
                if (data.Success>0)
                return new APIResponse { isSuccess = true,  ResponseMessage = data.ResponseMessage };

                return new APIResponse { isSuccess = false, ResponseMessage = data.ResponseMessage };

            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false,  ResponseMessage = "Some thing went wrong!" };
            }
        }


    }
}
