using HRMS_API.NotificationService.HubService;
using HRMS_API.NotificationService.ManageService;
using HRMS_Core.Notifications;
using HRMS_Core.VM.Employee;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace HRMS_API.Controllers.Employee
{
    [Route("api/[controller]")]
    [ApiController]
    public class BirthdayWishAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;

        public BirthdayWishAPIController(IUnitOfWork unitOfWork, IHubContext<NotificationRemainderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        [HttpPost("CreateBirthdayWish")]
        public async Task<APIResponse> CreateBirthdayWish([FromBody] vmCreateBirthdayWish model)
        {
            try
            {
                if (model == null || model.BirthdayEmployeeId <= 0 || model.WishedByEmployeeId <= 0 || string.IsNullOrWhiteSpace(model.Message))
                    return new APIResponse { isSuccess = false, ResponseMessage = "Wish details cannot be empty." };

                if (model.BirthdayEmployeeId == model.WishedByEmployeeId)
                    return new APIResponse { isSuccess = false, ResponseMessage = "You can't wish yourself." };

                var result = await _unitOfWork.BirthdayWishRepository.CreateBirthdayWish(model);
                if (result.Id is null or 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to save wish. Please try again." };

                var wisher = await _unitOfWork.EmployeeManageRepository.GetEmployeeById(model.WishedByEmployeeId);
                await NotifyAsync(
                    senderId: model.WishedByEmployeeId,
                    receiverId: model.BirthdayEmployeeId,
                    message: $"{wisher?.FullName} wished you a happy birthday!",
                    notificationType: NotificationType.BirthdayWish,
                    affectedId: result.Id
                );

                return new APIResponse { isSuccess = true, Data = result.Id, ResponseMessage = "Wish posted." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to save wish. Please try again later." };
            }
        }

        [HttpDelete("DeleteBirthdayWish/{id}/{wishedByEmployeeId}")]
        public async Task<APIResponse> DeleteBirthdayWish(int id, int wishedByEmployeeId)
        {
            try
            {
                var result = await _unitOfWork.BirthdayWishRepository.DeleteBirthdayWish(id, wishedByEmployeeId);
                if (result.Id is null or 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete wish." };

                return new APIResponse { isSuccess = true, ResponseMessage = "Wish deleted." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete wish. Please try again later." };
            }
        }

        [HttpGet("GetBirthdayWishes/{birthdayEmployeeId}/{companyId}/{requestingEmployeeId}")]
        public async Task<APIResponse> GetBirthdayWishes(int birthdayEmployeeId, int companyId, int requestingEmployeeId)
        {
            try
            {
                var data = await _unitOfWork.BirthdayWishRepository.GetBirthdayWishesByEmployee(birthdayEmployeeId, companyId, DateTime.UtcNow.Year, requestingEmployeeId);
                return new APIResponse { isSuccess = true, Data = data ?? new List<vmBirthdayWish>(), ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpPost("ToggleBirthdayWishLike")]
        public async Task<APIResponse> ToggleBirthdayWishLike([FromBody] vmToggleBirthdayWishLike model)
        {
            try
            {
                if (model == null || model.BirthdayWishId <= 0 || model.EmployeeId <= 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Like details cannot be empty." };

                var result = await _unitOfWork.BirthdayWishRepository.ToggleBirthdayWishLike(model);

                if (result.IsLikedNow && result.WishedByEmployeeId.HasValue)
                {
                    var liker = await _unitOfWork.EmployeeManageRepository.GetEmployeeById(model.EmployeeId);
                    await NotifyAsync(
                        senderId: model.EmployeeId,
                        receiverId: result.WishedByEmployeeId.Value,
                        message: $"{liker?.FullName} liked your birthday wish.",
                        notificationType: NotificationType.BirthdayWishLike,
                        affectedId: model.BirthdayWishId
                    );
                }

                return new APIResponse { isSuccess = true, Data = result.IsLikedNow, ResponseMessage = result.IsLikedNow ? "Liked." : "Like removed." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to update like. Please try again later." };
            }
        }

        [HttpGet("GetBirthdayWishComments/{birthdayWishId}")]
        public async Task<APIResponse> GetBirthdayWishComments(int birthdayWishId)
        {
            try
            {
                var data = await _unitOfWork.BirthdayWishRepository.GetBirthdayWishComments(birthdayWishId);
                return new APIResponse { isSuccess = true, Data = data ?? new List<vmBirthdayWishComment>(), ResponseMessage = "Records fetched successfully." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to retrieve records. Please try again later." };
            }
        }

        [HttpPost("CreateBirthdayWishComment")]
        public async Task<APIResponse> CreateBirthdayWishComment([FromBody] vmCreateBirthdayWishComment model)
        {
            try
            {
                if (model == null || model.BirthdayWishId <= 0 || model.EmployeeId <= 0 || string.IsNullOrWhiteSpace(model.CommentText))
                    return new APIResponse { isSuccess = false, ResponseMessage = "Comment cannot be empty." };

                var result = await _unitOfWork.BirthdayWishRepository.CreateBirthdayWishComment(model);
                if (result.Id is null or 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to save comment. Please try again." };

                if (result.WishedByEmployeeId.HasValue)
                {
                    var commenter = await _unitOfWork.EmployeeManageRepository.GetEmployeeById(model.EmployeeId);
                    await NotifyAsync(
                        senderId: model.EmployeeId,
                        receiverId: result.WishedByEmployeeId.Value,
                        message: $"{commenter?.FullName} commented on your birthday wish.",
                        notificationType: NotificationType.BirthdayWishComment,
                        affectedId: model.BirthdayWishId
                    );
                }

                return new APIResponse { isSuccess = true, Data = result.Id, ResponseMessage = "Comment posted." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to save comment. Please try again later." };
            }
        }

        [HttpDelete("DeleteBirthdayWishComment/{id}/{employeeId}")]
        public async Task<APIResponse> DeleteBirthdayWishComment(int id, int employeeId)
        {
            try
            {
                var result = await _unitOfWork.BirthdayWishRepository.DeleteBirthdayWishComment(id, employeeId);
                if (result.Id is null or 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete comment." };

                return new APIResponse { isSuccess = true, ResponseMessage = "Comment deleted." };
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, Data = ex.Message, ResponseMessage = "Unable to delete comment. Please try again later." };
            }
        }

        private async Task NotifyAsync(int senderId, int receiverId, string message, string notificationType, int? affectedId)
        {
            if (senderId == receiverId) return;

            try
            {
                var notification = new NotificationRemainders
                {
                    SenderId = senderId.ToString(),
                    ReceiverIds = receiverId.ToString(),
                    NotificationMessage = message,
                    NotificationType = notificationType,
                    NotificationAffectedId = affectedId
                };

                var saved = await _unitOfWork.NotificationRemainderRepository.CreateNotificationRemainder(notification);
                if (saved.Success > 0)
                {
                    notification.NotificationRemainderId = saved.Success;
                    var connections = NotificationRemainderConnectionManager.GetConnections(receiverId.ToString());
                    if (connections.Any())
                    {
                        await _hubContext.Clients.Clients(connections).SendAsync("ReceiveNotificationRemainder", notification);
                    }
                }
            }
            catch
            {
                // A notification failure must never fail the wish/like/comment action it's attached to.
            }
        }
    }
}
