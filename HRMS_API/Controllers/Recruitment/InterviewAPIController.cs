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
    public class InterviewAPIController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService _emailService;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;

        public InterviewAPIController(IUnitOfWork unitOfWork, EmailService emailService, IHubContext<NotificationRemainderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _hubContext = hubContext;
        }

        [HttpGet("GetAllInterviews")]
        public async Task<APIResponse> GetAllInterviews(int? candidateApplicationId)
        {
            try { return await _unitOfWork.InterviewRepository.GetAllInterviews(candidateApplicationId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve interviews. Please try again later." }; }
        }

        [HttpGet("GetInterviewById/{interviewId}")]
        public async Task<APIResponse> GetInterviewById(int interviewId)
        {
            try { return await _unitOfWork.InterviewRepository.GetInterviewById(interviewId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve interview. Please try again later." }; }
        }

        [HttpGet("GetCalendarEvents")]
        public async Task<APIResponse> GetCalendarEvents(int? companyId, DateTime fromDate, DateTime toDate)
        {
            try { return await _unitOfWork.InterviewRepository.GetCalendarEvents(companyId, fromDate, toDate); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve calendar events. Please try again later." }; }
        }

        [HttpGet("GetInterviewsByPanelistEmployeeId/{employeeId}")]
        public async Task<APIResponse> GetInterviewsByPanelistEmployeeId(int employeeId)
        {
            try { return await _unitOfWork.InterviewRepository.GetInterviewsByPanelistEmployeeId(employeeId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve interviews. Please try again later." }; }
        }

        // Schedules the interview, assigns panelists, and notifies each panelist (in-app +
        // email) — same side-effect shape as Phase 1's CandidateApplicationAPIController.Create.
        [HttpPost("ScheduleInterview")]
        public async Task<APIResponse> ScheduleInterview([FromBody] ScheduleInterviewRequest model)
        {
            try
            {
                if (model?.Interview == null || model.Interview.CandidateApplicationId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Candidate application is required." };

                model.Interview.CreatedDate = DateTime.UtcNow;
                var createResult = await _unitOfWork.InterviewRepository.CreateInterview(model.Interview);
                if (!createResult.isSuccess) return createResult;

                int interviewId = Convert.ToInt32(createResult.Data);

                if (model.PanelistEmployeeIds != null)
                {
                    foreach (var employeeId in model.PanelistEmployeeIds.Distinct())
                    {
                        await _unitOfWork.InterviewRepository.AddPanelist(new InterviewPanelist
                        {
                            InterviewId = interviewId,
                            EmployeeId = employeeId,
                            CreatedBy = model.Interview.CreatedBy
                        });

                        var notification = new NotificationRemainders
                        {
                            NotificationMessage = $"You have been assigned as a panelist for an interview ({model.Interview.RoundName}) on {model.Interview.ScheduledDate:dd-MMM-yyyy HH:mm}.",
                            NotificationTime = DateTime.UtcNow,
                            ReceiverIds = employeeId.ToString(),
                            NotificationType = NotificationType.InterviewScheduled,
                            NotificationAffectedId = interviewId
                        };

                        var saved = await _unitOfWork.NotificationRemainderRepository.CreateNotificationRemainder(notification);
                        if (saved.Success > 0)
                        {
                            notification.NotificationRemainderId = saved.Success;
                            var connections = NotificationRemainderConnectionManager.GetConnections(employeeId.ToString());
                            if (connections.Any())
                                await _hubContext.Clients.Clients(connections).SendAsync("ReceiveNotificationRemainder", notification);
                        }
                    }
                }

                var candidateResponse = await _unitOfWork.CandidateApplicationRepository.GetCandidateApplicationById(model.Interview.CandidateApplicationId);
                if (candidateResponse?.Data is CandidateApplication application && !string.IsNullOrWhiteSpace(application.Email))
                {
                    var emailRequest = new EmailRequest
                    {
                        ToEmails = new List<string> { application.Email },
                        Subject = $"Interview Scheduled - {model.Interview.RoundName}",
                        TemplateName = "InterviewScheduledEmailTemplate.html",
                        Placeholders = new Dictionary<string, string>
                        {
                            { "CandidateName", application.FullName ?? "Candidate" },
                            { "RoundName", model.Interview.RoundName ?? "N/A" },
                            { "ScheduledDate", model.Interview.ScheduledDate.ToString("dd-MM-yyyy HH:mm") },
                            { "Mode", model.Interview.Mode ?? "N/A" },
                            { "MeetingLink", model.Interview.MeetingLink ?? "N/A" },
                            { "Location", model.Interview.Location ?? "N/A" },
                            { "CompanyName", "Relay Express" }
                        }
                    };
                    await _emailService.SendEmailAsync(emailRequest);
                }

                return createResult;
            }
            catch (Exception ex)
            {
                return new APIResponse { isSuccess = false, ResponseMessage = "Unable to schedule interview. Please try again later.", Data = ex.Message };
            }
        }

        [HttpPost("UpdateInterview")]
        public async Task<APIResponse> UpdateInterview([FromBody] Interview model)
        {
            try
            {
                if (model == null || model.InterviewId == 0)
                    return new APIResponse { isSuccess = false, ResponseMessage = "Please select a valid interview." };
                return await _unitOfWork.InterviewRepository.UpdateInterview(model);
            }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to update interview. Please try again later." }; }
        }

        [HttpDelete("CancelInterview")]
        public async Task<APIResponse> CancelInterview(DeleteRecordVM model)
        {
            try { return await _unitOfWork.InterviewRepository.DeleteInterview(model); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to cancel interview. Please try again later." }; }
        }

        [HttpGet("GetPanelistsByInterviewId/{interviewId}")]
        public async Task<APIResponse> GetPanelistsByInterviewId(int interviewId)
        {
            try { return await _unitOfWork.InterviewRepository.GetPanelistsByInterviewId(interviewId); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to retrieve panelists. Please try again later." }; }
        }

        [HttpDelete("RemovePanelist")]
        public async Task<APIResponse> RemovePanelist(DeleteRecordVM model)
        {
            try { return await _unitOfWork.InterviewRepository.RemovePanelist(model); }
            catch { return new APIResponse { isSuccess = false, ResponseMessage = "Unable to remove panelist. Please try again later." }; }
        }
    }

    public class ScheduleInterviewRequest
    {
        public Interview Interview { get; set; } = new();
        public List<int>? PanelistEmployeeIds { get; set; }
    }
}
