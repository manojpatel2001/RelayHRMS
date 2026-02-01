using Hangfire;
using HRMS_Core.Services;
using HRMS_Core.VM.ApprovalManagement;
using HRMS_Core.VM.EmailService;
using HRMS_Infrastructure.Interface;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text;

namespace HRMS_API.Services
{
    public class AutoJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly EmailService _emailService;


        public AutoJobService(IUnitOfWork unitOfWork, EmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _emailService = emailService;
        }

        public async Task ScheduleDailyCheckProbation()
        {
            try
            {
                // Fetch the email report configuration for "Daily Left Employee Report"
                var check = await _unitOfWork.ApprovalManagementRepository
                    .AutomateProbationEndApprovalRequests(1);

                var escalated =await _unitOfWork.ApprovalManagementRepository.EscalatePendingApprovalRequests();
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


                            var emailReport = await _unitOfWork.EmailReportRepository.GetEmailSendTime(EmailReportType.EscalatedReport.ToString());
                            if(emailReport == null|| string.IsNullOrEmpty(emailReport.ToEmails))
                            {
                                return;
                            }
                            
                           
                            var Subject = $"Probation Escalation Notification";
                            var TemplateName = "ApprovalEscalatedEmailTemplate.html";

                            var emailRequest = new EmailRequest
                            {
                                ToEmails = emailReport.ToEmails.Split(',').ToList(),
                                BccEmails = emailReport?.BccEmails?.Split(',').ToList(),
                                CcEmails= emailReport?.CcEmails?.Split(',').ToList(),
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

        public void StartAutoJobService()
        {
            RecurringJob.AddOrUpdate(
                "probation-schedule-check",
                () => ScheduleDailyCheckProbation(),
                "*/5 * * * *"
            );

        }
    }
}
