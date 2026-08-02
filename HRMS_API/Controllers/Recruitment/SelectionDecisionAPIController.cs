using HRMS_API.NotificationService.HubService;
using HRMS_API.NotificationService.ManageService;
using HRMS_Core.Notifications;
using HRMS_Core.Recruitment;
using HRMS_Core.VM;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace HRMS_API.Controllers.Recruitment
{
    [Route("api/[controller]")]
    [ApiController]
    public class SelectionDecisionAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;

        public SelectionDecisionAPIController(IUnitOfWork unitOfWork, IHubContext<NotificationRemainderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        [HttpPost("RecordDecision")]
        public async Task<APIResponse> RecordDecision([FromBody] SelectionDecision model)
        {
            try
            {
                if (model == null || model.CandidateApplicationId == 0 || string.IsNullOrWhiteSpace(model.DecisionStatus))
                    return new APIResponse { isSuccess = false, ResponseMessage = "Candidate application and decision status are required." };

                var result = await _unitOfWork.SelectionDecisionRepository.CreateSelectionDecision(model);

                if (result.isSuccess)
                {
                    var applicationResponse = await _unitOfWork.CandidateApplicationRepository.GetCandidateApplicationById(model.CandidateApplicationId);
                    if (applicationResponse?.Data is CandidateApplication application)
                    {
                        var positionResponse = await _unitOfWork.JobPositionRepository.GetJobPositionById(application.JobPositionId);
                        if (positionResponse?.Data is JobPositionMaster position && position.OwnerRecruiterId.HasValue)
                        {
                            var notification = new NotificationRemainders
                            {
                                NotificationMessage = $"Selection decision for {application.FullName}: {model.DecisionStatus}",
                                NotificationTime = DateTime.UtcNow,
                                SenderId = model.DecidedBy?.ToString(),
                                ReceiverIds = position.OwnerRecruiterId.ToString(),
                                NotificationType = NotificationType.SelectionDecisionMade,
                                NotificationAffectedId = model.CandidateApplicationId
                            };

                            var saved = await _unitOfWork.NotificationRemainderRepository.CreateNotificationRemainder(notification);
                            if (saved.Success > 0)
                            {
                                notification.NotificationRemainderId = saved.Success;
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
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to record selection decision. Please try again later." };
            }
        }

        [HttpPost("UpdateDecision")]
        public async Task<APIResponse> UpdateDecision([FromBody] SelectionDecision model)
        {
            try
            {
                if (model == null || model.SelectionDecisionId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid decision record." };
                return await _unitOfWork.SelectionDecisionRepository.UpdateSelectionDecision(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update selection decision. Please try again later." }; }
        }

        [HttpDelete("DeleteDecision")]
        public async Task<APIResponse> DeleteDecision(DeleteRecordVM model)
        {
            try { return await _unitOfWork.SelectionDecisionRepository.DeleteSelectionDecision(model); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete selection decision. Please try again later." }; }
        }

        [HttpGet("GetByCandidateApplicationId/{candidateApplicationId}")]
        public async Task<APIResponse> GetByCandidateApplicationId(int candidateApplicationId)
        {
            try { return await _unitOfWork.SelectionDecisionRepository.GetSelectionDecisionByCandidateApplicationId(candidateApplicationId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve selection decision. Please try again later." }; }
        }
    }
}
