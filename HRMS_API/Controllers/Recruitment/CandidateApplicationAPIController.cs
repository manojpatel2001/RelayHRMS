using HRMS_API.NotificationService.HubService;
using HRMS_API.NotificationService.ManageService;
using HRMS_API.Services;
using HRMS_Core.Notifications;
using HRMS_Core.Recruitment;
using HRMS_Core.Services;
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
    public class CandidateApplicationAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileUploadService _fileUploadService;
        private readonly EmailService _emailService;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;
        private readonly ResumeScreeningService _resumeScreeningService;

        public CandidateApplicationAPIController(
            IUnitOfWork unitOfWork,
            FileUploadService fileUploadService,
            EmailService emailService,
            IHubContext<NotificationRemainderHub> hubContext,
            ResumeScreeningService resumeScreeningService)
        {
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
            _emailService = emailService;
            _hubContext = hubContext;
            _resumeScreeningService = resumeScreeningService;
        }

        [HttpPost("GetAllCandidateApplications")]
        public async Task<APIResponse> GetAllCandidateApplications([FromBody] JobPositionFilter filter)
        {
            try
            {
                return await _unitOfWork.CandidateApplicationRepository.GetAllCandidateApplications(filter?.JobPositionId);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve candidate applications. Please try again later." };
            }
        }

        [HttpGet("GetCandidateApplicationById/{candidateApplicationId}")]
        public async Task<APIResponse> GetCandidateApplicationById(int candidateApplicationId)
        {
            try
            {
                return await _unitOfWork.CandidateApplicationRepository.GetCandidateApplicationById(candidateApplicationId);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve candidate application. Please try again later." };
            }
        }

        [HttpGet("GetCandidateApplicationsByJobPositionId/{jobPositionId}")]
        public async Task<APIResponse> GetCandidateApplicationsByJobPositionId(int jobPositionId)
        {
            try
            {
                return await _unitOfWork.CandidateApplicationRepository.GetAllCandidateApplications(jobPositionId);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve candidate applications. Please try again later." };
            }
        }

        [HttpGet("GetKanbanBoardData/{jobPositionId}")]
        public async Task<APIResponse> GetKanbanBoardData(int jobPositionId)
        {
            try
            {
                return await _unitOfWork.CandidateApplicationRepository.GetKanbanBoardData(jobPositionId);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve pipeline board. Please try again later." };
            }
        }

        // Full application-intake chain: duplicate pre-check -> resume upload -> insert with
        // initial pipeline stage -> rule-based screening -> candidate acknowledgement email ->
        // in-app notification to the position's recruiter. Mirrors the side-effect chain in
        // ManpowerRequisitionAPIController.CreateManpowerRequisition.
        [HttpPost("CreateCandidateApplication")]
        public async Task<APIResponse> CreateCandidateApplication([FromForm] CandidateApplication model, IFormFile? resumeFile)
        {
            try
            {
                if (model == null || model.CandidateId == 0 || model.JobPositionId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Candidate and Job Position are required." };

                if (resumeFile != null && resumeFile.Length > 0)
                {
                    var fileUrl = await _fileUploadService.UploadAndReplaceDocumentAsync(resumeFile, "uploads/candidate-resume", model.ResumeUrl);
                    if (string.IsNullOrEmpty(fileUrl))
                        return new APIResponse { isSuccess = false, ResponseMessage = "Resume upload failed. Please try again later." };
                    model.ResumeUrl = fileUrl;
                }

                var stagesResponse = await _unitOfWork.CandidatePipelineRepository.GetAllPipelineStages(null);
                var stages = stagesResponse?.Data as List<CandidatePipelineStage>;
                var appliedStage = stages?.OrderBy(s => s.SortOrder).FirstOrDefault();
                model.CurrentPipelineStageId = appliedStage?.CandidatePipelineStageId ?? model.CurrentPipelineStageId;

                model.CreatedDate = DateTime.UtcNow;
                var createResult = await _unitOfWork.CandidateApplicationRepository.CreateCandidateApplication(model);
                if (!createResult.isSuccess) return createResult;

                int newApplicationId = Convert.ToInt32(createResult.Data);

                await _resumeScreeningService.ScreenCandidateApplicationAsync(newApplicationId, model.CreatedBy);

                var candidateResponse = await _unitOfWork.CandidateRepository.GetCandidateById(model.CandidateId);
                var candidate = candidateResponse?.Data as Candidate;
                var positionResponse = await _unitOfWork.JobPositionRepository.GetJobPositionById(model.JobPositionId);
                var position = positionResponse?.Data as JobPositionMaster;

                if (candidate != null && !string.IsNullOrWhiteSpace(candidate.Email))
                {
                    var emailRequest = new EmailRequest
                    {
                        ToEmails = new List<string> { candidate.Email },
                        Subject = $"Application Received - {position?.PositionTitle}",
                        TemplateName = "CandidateApplicationReceivedTemplate.html",
                        Placeholders = new Dictionary<string, string>
                        {
                            { "CandidateName", candidate.FullName ?? candidate.FirstName ?? "Candidate" },
                            { "PositionTitle", position?.PositionTitle ?? "N/A" },
                            { "ApplicationDate", (model.ApplicationDate ?? DateTime.UtcNow).ToString("dd-MM-yyyy") },
                            { "ApplicationSource", model.ApplicationSource ?? "N/A" },
                            { "CompanyName", "Relay Express" }
                        }
                    };
                    await _emailService.SendEmailAsync(emailRequest);
                }

                if (position?.OwnerRecruiterId != null)
                {
                    var notification = new NotificationRemainders
                    {
                        NotificationMessage = $"{candidate?.FullName} applied for {position.PositionTitle}",
                        NotificationTime = DateTime.UtcNow,
                        SenderId = model.CreatedBy?.ToString(),
                        ReceiverIds = position.OwnerRecruiterId.ToString(),
                        NotificationType = NotificationType.CandidateApplication,
                        NotificationAffectedId = newApplicationId
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

                return createResult;
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to create candidate application. Please try again later.", Data = ex.Message };
            }
        }

        [HttpPost("UpdateCandidateApplication")]
        public async Task<APIResponse> UpdateCandidateApplication([FromBody] CandidateApplication model)
        {
            try
            {
                if (model == null || model.CandidateApplicationId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid candidate application." };

                return await _unitOfWork.CandidateApplicationRepository.UpdateCandidateApplication(model);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update candidate application. Please try again later." };
            }
        }

        [HttpDelete("DeleteCandidateApplication")]
        public async Task<APIResponse> DeleteCandidateApplication(DeleteRecordVM model)
        {
            try
            {
                return await _unitOfWork.CandidateApplicationRepository.DeleteCandidateApplication(model);
            }
            catch
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to delete candidate application. Please try again later." };
            }
        }
    }

    public class JobPositionFilter
    {
        public int? JobPositionId { get; set; }
    }
}
