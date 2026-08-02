using Hangfire;
using HRMS_API.NotificationService.HubService;
using HRMS_API.NotificationService.ManageService;
using HRMS_Core.Notifications;
using HRMS_Core.Services;
using HRMS_Core.VM.ApprovalManagement;
using HRMS_Core.VM.EmailService;
using HRMS_Core.VM.Probations;
using HRMS_Infrastructure.Interface;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace HRMS_API.Services
{
    public class AutoJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService _emailService;
        private readonly IHubContext<NotificationRemainderHub> _hubContext;

        public AutoJobService(
            IUnitOfWork unitOfWork,
            EmailService emailService,
            IHubContext<NotificationRemainderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _hubContext = hubContext;
        }

        public async Task ScheduleDailyCheckProbation()
        {
            try
            {
                // Fetch the email report configuration for "Daily Left Employee Report"
                var check = await _unitOfWork.ApprovalManagementRepository
                    .AutomateProbationEndApprovalRequests(1);

                var escalatedManpower = await _unitOfWork.ManpowerRequisitionRepository.EscalatePendingManpowerApprovalRequests();
                var escalated = await _unitOfWork.ApprovalManagementRepository.EscalatePendingApprovalRequests();

                if (escalated.IsSuccess)
                {
                    if (!string.IsNullOrEmpty(escalated.EscalatedData))
                    {
                        var escalatedList = JsonConvert.DeserializeObject<List<EscalationMailModel>>(escalated.EscalatedData);

                        var groupedByCompany = escalatedList
                            .GroupBy(x => x.CompanyName)
                            .ToList();


                        foreach (var companyGroup in groupedByCompany)
                        {
                            var employeeRows = new StringBuilder();

                            foreach (var emp in companyGroup)
                            {
                                employeeRows.Append($@"
                                        <tr>
                                            <td>{emp.EmployeeName ?? "N/A"}</td>
                                            <td>{emp.EmployeeCode ?? "N/A"}</td>
                                            <td>
                                                {emp.PreviousApprover ?? "N/A"}<br/>
                                                <small>{emp.PreviousApproverCode ?? "N/A"}</small>
                                            </td>
                                            <td>
                                                {emp.NextApprover ?? "N/A"}<br/>
                                                <small>{emp.NextApproverCode ?? "N/A"}</small>
                                            </td>
                                            <td>Level {emp.PreviousLevelNo}</td>
                                            <td>Level {emp.NextLevelNo}</td>
                                        </tr>"
                                );
                            }

                            var placeholders = new Dictionary<string, string>
                            {
                                { "CompanyName", companyGroup.Key ?? "N/A" },
                                { "Year", DateTime.Now.Year.ToString() },
                                { "EscalatedEmployeeList", employeeRows.ToString() }
                            };



                            var emailReport = await _unitOfWork.EmailReportRepository
                    .GetEmailSendTime(EmailReportType.EscalatedReport.ToString());

                            // ✅ NULL + SERVER CHECK
                            if (emailReport == null|| string.IsNullOrEmpty(emailReport.ToEmails)
                                || emailReport.EmailSendTime == null
                                )
                            {
                                Console.WriteLine("⛔ Scheduler skipped");
   
                                return;
                            }
                            var Subject = $"Probation Escalation Notification";
                            var TemplateName = "ApprovalEscalatedEmailTemplate.html";

                            var emailRequest = new EmailRequest
                            {
                                ToEmails = emailReport.ToEmails.Split(',').ToList(),
                                BccEmails = emailReport?.BccEmails?.Split(',').ToList(),
                                CcEmails = emailReport?.CcEmails?.Split(',').ToList(),
                                Subject = Subject,
                                TemplateName = TemplateName,
                                Placeholders = placeholders
                            };


                            var reportingEmailLogger = new EmailLogger
                            {
                                ToEmail = emailReport.ToEmails,
                                BCCEmail = emailReport?.BccEmails,
                                CCEmail = emailReport?.CcEmails,
                                Subject = emailRequest.Subject,
                                Body = emailRequest.TemplateName,
                                Status = EmailStatus.Pending,
                                SentAt = DateTime.UtcNow,
                                Comments = "Email ready for sent"

                            };

                            await _unitOfWork.EmailLoggerRepository.ManageEmailLoggerAsync(reportingEmailLogger, "CREATE");

                            //Send the email
                            bool checkStatus = await _emailService.SendEmailAsync(emailRequest);



                        }




                    }

                }


            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error scheduling Daily probation job: {ex.Message}");
            }
        }

        public async Task ScheduleDailyProbationNotification()
        {
            try
            {
                var alerts = await _unitOfWork.ApprovalManagementRepository
                                    .GetTodayProbationAlertsAsync();

                Console.WriteLine($"🔔 Alerts count: {alerts?.Count}");

                if (alerts == null || !alerts.Any())
                {
                    Console.WriteLine("✅ No probation notifications due today.");
                    return;
                }

                foreach (var alert in alerts)
                {
                    Console.WriteLine($"🔔 Processing alert for: {alert.FullName}, ReceiverUserId: {alert.ReceiverUserId}");

                    int receiverId = alert.ReceiverUserId ?? alert.ReportingManagerId;

                    Console.WriteLine($"🔔 ReceiverID: {receiverId}");

                    var notification = new NotificationRemainders()
                    {
                        SenderId = alert.EmployeeId.ToString(),
                        ReceiverIds = receiverId.ToString(),
                        NotificationMessage = GetNotificationMessage(alert),
                        NotificationType = NotificationType.PerformanceEvaluation,
                        NotificationAffectedId = alert.EmployeeId,
                        IsRead = false,
                        NotificationTime = DateTime.Now,
                        IsDeleted = false
                    };

                    Console.WriteLine($"🔔 Saving notification...");

                    var savedNotification = await _unitOfWork.NotificationRemainderRepository
                                                  .CreateNotificationRemainder(notification);

                    Console.WriteLine($"🔔 Save result: {savedNotification?.Success}");

                    if (savedNotification.Success > 0)
                    {
                        Console.WriteLine($"✅ Notification saved! ID: {savedNotification.Success}");

                        notification.NotificationRemainderId = savedNotification.Success;

                        // ✅ SignalR - Real time notification agar user online hai
                        var connections = NotificationRemainderConnectionManager
                                            .GetConnections(receiverId.ToString())
                                            as IEnumerable<string>
                                            ?? Enumerable.Empty<string>();

                        if (connections.Any())
                        {
                            await _hubContext.Clients.Clients(connections)
                                  .SendAsync("ReceiveNotificationRemainder", notification);

                            Console.WriteLine($"✅ Real-time notification sent to ReceiverID: {receiverId}");
                        }
                        else
                        {
                            Console.WriteLine($"⚠️ ReceiverID: {receiverId} is offline. Notification saved in DB only.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"❌ Notification save FAILED for: {alert.FullName}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
                Console.WriteLine($"❌ StackTrace: {ex.StackTrace}");
            }
        }
        // Offer Approval escalation sweep — mirrors ScheduleDailyCheckProbation's generic
        // escalation handling above, but scoped to the self-contained Offer Approval tables
        // (see HRMS_API\Services\OfferApprovalService.cs). Reuses the same, already-generic
        // ApprovalEscalatedEmailTemplate.html and also pushes an in-app SignalR notification
        // directly to the overdue approver.
        public async Task CheckOfferApprovalEscalations()
        {
            try
            {
                var overdueResponse = await _unitOfWork.OfferApprovalRepository.GetAndMarkOverdueLevels();
                var overdueLevels = overdueResponse?.Data as List<HRMS_Core.Recruitment.OfferApprovalRequestLevel>;
                if (overdueLevels == null || overdueLevels.Count == 0) return;

                foreach (var level in overdueLevels)
                {
                    if (level.ApproverEmployeeId.HasValue)
                    {
                        var notification = new NotificationRemainders
                        {
                            NotificationMessage = $"Offer approval for {level.CandidateName} ({level.PositionTitle}) is overdue at your level.",
                            NotificationTime = DateTime.UtcNow,
                            ReceiverIds = level.ApproverEmployeeId.Value.ToString(),
                            NotificationType = HRMS_Core.Notifications.NotificationType.OfferApprovalRequest,
                            NotificationAffectedId = level.OfferId
                        };

                        var saved = await _unitOfWork.NotificationRemainderRepository.CreateNotificationRemainder(notification);
                        if (saved.Success > 0)
                        {
                            notification.NotificationRemainderId = saved.Success;
                            var connections = NotificationRemainderConnectionManager.GetConnections(level.ApproverEmployeeId.Value.ToString());
                            if (connections.Any())
                                await _hubContext.Clients.Clients(connections).SendAsync("ReceiveNotificationRemainder", notification);
                        }
                    }
                }

                var groupedByCompany = overdueLevels.GroupBy(x => x.CompanyId);
                foreach (var companyGroup in groupedByCompany)
                {
                    var rows = new StringBuilder();
                    foreach (var level in companyGroup)
                    {
                        rows.Append($@"
                                <tr>
                                    <td>{level.CandidateName ?? "N/A"}</td>
                                    <td>{level.PositionTitle ?? "N/A"}</td>
                                    <td>N/A</td>
                                    <td>Approver #{level.ApproverEmployeeId?.ToString() ?? "N/A"}</td>
                                    <td>Level {level.LevelNo - 1}</td>
                                    <td>Level {level.LevelNo}</td>
                                </tr>"
                        );
                    }

                    var emailReport = await _unitOfWork.EmailReportRepository.GetEmailSendTime(EmailReportType.EscalatedReport.ToString());
                    if (emailReport == null || string.IsNullOrEmpty(emailReport.ToEmails)) continue;

                    var placeholders = new Dictionary<string, string>
                    {
                        { "CompanyName", "N/A" },
                        { "Year", DateTime.Now.Year.ToString() },
                        { "EscalatedEmployeeList", rows.ToString() }
                    };

                    var emailRequest = new EmailRequest
                    {
                        ToEmails = emailReport.ToEmails.Split(',').ToList(),
                        BccEmails = emailReport?.BccEmails?.Split(',').ToList(),
                        CcEmails = emailReport?.CcEmails?.Split(',').ToList(),
                        Subject = "Offer Approval Escalation Notification",
                        TemplateName = "ApprovalEscalatedEmailTemplate.html",
                        Placeholders = placeholders
                    };

                    await _emailService.SendEmailAsync(emailRequest);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error checking offer approval escalations: {ex.Message}");
            }
        }

        private string GetNotificationMessage(ProbationAlertDto alert)
        {
            return alert.AlertStatus switch
            {
                "NOTIFY_MANAGER" =>
                    $"{alert.FullName} ({alert.EmployeeCode})'s probation period is ending in " +
                    $"{alert.DaysRemainingToConfirmation} days on {alert.ProbationEndDate:dd-MMM-yyyy}. " +
                    $"Please complete the probation review within 4 working days.",

                "ESCALATED_TO_HR" =>
                    $"{alert.FullName} ({alert.EmployeeCode})'s probation review has not been " +
                    $"completed by the Reporting Manager within 4 working days. " +
                    $"Probation End Date: {alert.ProbationEndDate:dd-MMM-yyyy}. Immediate action required.",

                "HRD_ALERT" =>
                    $"URGENT: {alert.FullName} ({alert.EmployeeCode})'s probation period is ending " +
                    $"tomorrow on {alert.ProbationEndDate:dd-MMM-yyyy}. " +
                    $"Please take necessary action today.",

                _ => "Probation period alert."
            };
        }

         //✅ Hangfire Jobs
        public void StartAutoJobService()
        {
           
            // Existing job
            RecurringJob.AddOrUpdate(
                     "probation-schedule-check",
                     () => ScheduleDailyCheckProbation(),
                     "*/5 * * * *"
                 );

            //// ✅ Daily 10 AM
            RecurringJob.AddOrUpdate(
                "probation-daily-notification",
                () => ScheduleDailyProbationNotification(),
                "30 4 * * *"
            );

            // ✅ Offer approval escalation sweep (every 30 minutes)
            RecurringJob.AddOrUpdate(
                "offer-approval-escalation-check",
                () => CheckOfferApprovalEscalations(),
                "*/30 * * * *"
            );
        }



    }
}
