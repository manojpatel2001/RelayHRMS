using HRMS_API.NotificationService.HubService;
using HRMS_API.NotificationService.ManageService;
using HRMS_Core.Notifications;
using HRMS_Core.Recruitment;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace HRMS_API.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class CandidatePipelineAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;

        public CandidatePipelineAPIController(IUnitOfWork unitOfWork, IHubContext<NotificationRemainderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        [HttpGet("GetAllPipelineStages/{companyId?}")]
        public async Task<APIResponse> GetAllPipelineStages(int? companyId)
        {
            try
            {
                return await _unitOfWork.CandidatePipelineRepository.GetAllPipelineStages(companyId);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve pipeline stages. Please try again later." };
            }
        }

        [HttpPost("ManagePipelineStage")]
        public async Task<APIResponse> ManagePipelineStage([FromBody] CandidatePipelineStage model)
        {
            try
            {
                if (model.CandidatePipelineStageId > 0)
                    return await _unitOfWork.CandidatePipelineRepository.UpdatePipelineStage(model);

                return await _unitOfWork.CandidatePipelineRepository.CreatePipelineStage(model);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to save pipeline stage. Please try again later." };
            }
        }

        [HttpPut("MoveCandidateToStage")]
        public async Task<APIResponse> MoveCandidateToStage([FromBody] MoveCandidatePipelineRequest model)
        {
            try
            {
                var result = await _unitOfWork.CandidatePipelineRepository.MoveCandidateToStage(
                    model.CandidateApplicationId, model.ToStageId, model.Remarks, model.ActionBy);

                if (result.isSuccess)
                {
                    var applicationResponse = await _unitOfWork.CandidateApplicationRepository.GetCandidateApplicationById(model.CandidateApplicationId);
                    var application = applicationResponse?.Data as CandidateApplication;
                    if (application != null)
                    {
                        var positionResponse = await _unitOfWork.JobPositionRepository.GetJobPositionById(application.JobPositionId);
                        var position = positionResponse?.Data as JobPositionMaster;

                        if (position?.OwnerRecruiterId != null)
                        {
                            var notification = new NotificationRemainders
                            {
                                NotificationMessage = $"{application.FullName} moved to a new pipeline stage for {position.PositionTitle}",
                                NotificationTime = DateTime.UtcNow,
                                SenderId = model.ActionBy?.ToString(),
                                ReceiverIds = position.OwnerRecruiterId.ToString(),
                                NotificationType = NotificationType.CandidatePipelineStageChange,
                                NotificationAffectedId = model.CandidateApplicationId
                            };

                            var savedNotification = await _unitOfWork.NotificationRemainderRepository.CreateNotificationRemainder(notification);
                            if (savedNotification.Success > 0)
                            {
                                notification.NotificationRemainderId = savedNotification.Success;
                                var connections = NotificationRemainderConnectionManager.GetConnections(position.OwnerRecruiterId.Value.ToString());
                                if (connections.Any())
                                    await _hubContext.Clients.Clients(connections).SendAsync("ReceiveNotificationRemainder", notification);
                            }
                        }
                    }
                }

                return result;
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to move candidate to the new stage. Please try again later." };
            }
        }

        [HttpGet("GetPipelineHistoryByApplicationId/{candidateApplicationId}")]
        public async Task<APIResponse> GetPipelineHistoryByApplicationId(int candidateApplicationId)
        {
            try
            {
                return await _unitOfWork.CandidatePipelineRepository.GetPipelineHistoryByApplicationId(candidateApplicationId);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve pipeline history. Please try again later." };
            }
        }
    }

    public class MoveCandidatePipelineRequest
    {
        public int CandidateApplicationId { get; set; }
        public int ToStageId { get; set; }
        public string? Remarks { get; set; }
        public int? ActionBy { get; set; }
    }
}
