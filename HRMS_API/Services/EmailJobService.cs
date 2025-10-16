using ClosedXML.Excel;
using Hangfire;
using HRMS_Core.Services;
using HRMS_Core.VM.EmailService;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using System.Text;

namespace HRMS_API.Services
{
    public class EmailJobService
    {
        private readonly EmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly FileUploadService _fileUploadService;


        public EmailJobService(EmailService emailService, IUnitOfWork unitOfWork, FileUploadService fileUploadService)
        {
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _fileUploadService = fileUploadService;
        }

        /// <summary>
        /// Hangfire will call this method for recurring emails
        /// </summary>
        public async Task SendDailyEmailAsync(EmailReport? emailReport)
        {
            try
            {
                
                List<DailyAbsentReportResult>? AbsentReport = await _unitOfWork.EmailReportRepository.GetDailyAbsentReport();
                if (emailReport==null|| AbsentReport == null|| !AbsentReport.Any())
                    return;

                // Iterate over each reporting manager
                foreach (var manager in AbsentReport)
                {
                    // Generate Excel for this manager's team
                    string excelDownloadLink = await GenerateAndUploadExcel(manager.Employees, manager.ReportingManagerName);

                    // Build dynamic employee table rows for this manager's team
                    var employeeRows = new StringBuilder();
                    int rowNum = 0;
                    int i = 1;
                    foreach (var emp in manager.Employees)
                    {
                        rowNum++;
                        var backgroundColor = rowNum % 2 == 0 ? "#f8f9fa" : "#ffffff";
                        employeeRows.AppendLine($@"
                        <tr>
                             <td style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:{backgroundColor};'>{i}</td>
                            <td style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:{backgroundColor};'>{emp.EmployeeName}</td>
                            <td style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:{backgroundColor};'>{emp.EmployeeCode}</td>
                            <td style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:{backgroundColor};'>{emp.BranchName}</td>
                            <td style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:{backgroundColor};'>{emp.Attendance}</td>
                         </tr>");
                        i++;
                    }
                    // Add a row for the Excel download link
                    employeeRows.AppendLine($@"
                    <tr>
                        <td colspan='5' style='padding:10px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:#e9ecef;'>
                            <a href='{excelDownloadLink}' style='color: #0066cc; text-decoration: none;'>Download Absentee Report (Excel)</a>
                                      <p style=""margin: 0 0 10px 0; color: #666666; font-size: 12px; line-height: 1.5;"">
                                           The Absentee Report (Excel) will be available for download for up to 10 days from the report generation date..
                                        </p>
                        </td>
                    </tr>");
                    // Create the placeholder dictionary

                    var dateFormate = DateTime.Now.AddDays(-1).ToString("dd MMM yyyy");
                    var placeholders = new Dictionary<string, string>
                    {
                        { "Date", dateFormate },

                        { "ManagerName", manager.ReportingManagerName??"" },
                        { "HRContactNumber", emailReport.HRContactNumber??"" },
                        { "HRContactEmail", emailReport.HRContactEmail??"" },
                        { "EmployeeRows", employeeRows.ToString() }
                    };
                    // Prepare email request object
                    var ToEmails = $"{manager.ReportingManagerEmail},{emailReport.ToEmails}";
                    var emailRequest = new EmailRequest
                    {
                        ToEmails = ToEmails.Split(',').ToList(),
                        CcEmails = emailReport?.BccEmails?.Split(',').ToList(),
                        BccEmails = emailReport?.BccEmails?.Split(',').ToList(),
                        Subject = $"{emailReport.Subject} – {dateFormate}",
                        TemplateName = "DailyAbsentReportEmailTemplate.html",
                        Placeholders = placeholders
                    };
                    //Send the email
                    bool result = await _emailService.SendEmailAsync(emailRequest);
                    if (result)
                        Console.WriteLine($"✅ Daily Absentee email sent to {manager.ReportingManagerName}.");
                    else
                        Console.WriteLine($"⚠️ Email sending failed for {manager.ReportingManagerName}.");
                   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in SendDailyEmailAsync: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
        /// <summary>
        /// Schedules the daily email job with the time from the database (default: 8 AM)
        /// </summary>
        public async Task ScheduleDailyEmail()
        {
            try
            {
                EmailReport? emailReport = await _unitOfWork.EmailReportRepository.GetEmailSendTime(EmailReportType.DailyAbsentEmployeesReport.ToString());

                if (emailReport == null)
                {
                    return;
                }
                string cronExpression = "";
                if (emailReport.EmailSendTime == null)
                {
                    return;
                }
                else
                {
                     cronExpression = $"{emailReport.EmailSendTime.Value.Minutes} {emailReport.EmailSendTime.Value.Hours} * * *";

                }

                RecurringJob.RemoveIfExists("update-job-schedule-check");
                RecurringJob.AddOrUpdate(
                    "update-job-schedule-check",
                    () => SendDailyEmailAsync(emailReport),
                    cronExpression // Every 5 minutes
                );

            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error scheduling email job: {ex.Message}");
            }
        }

        public void StartScheduleDailyEmail()
        {
            RecurringJob.AddOrUpdate(
                "update-job-schedule-check",
                () => ScheduleDailyEmail(),
                "*/5 * * * *" // Every 5 minutes
            );
        }

       
        private async Task<string> GenerateAndUploadExcel(List<EmployeeRecord> employees, string managerName)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Absent Employees");

            // Add headers
            worksheet.Cell(1, 1).Value = "S.No";
            worksheet.Cell(1, 2).Value = "Employee Name";
            worksheet.Cell(1, 3).Value = "Employee Code";
            worksheet.Cell(1, 4).Value = "Branch";
            worksheet.Cell(1, 5).Value = "Attendance";

            // Add data
            for (int i = 0; i < employees.Count; i++)
            {
                var emp = employees[i];
                worksheet.Cell(i + 2, 1).Value = i + 1;
                worksheet.Cell(i + 2, 2).Value = emp.EmployeeName;
                worksheet.Cell(i + 2, 3).Value = emp.EmployeeCode;
                worksheet.Cell(i + 2, 4).Value = emp.BranchName;
                worksheet.Cell(i + 2, 5).Value = emp.Attendance;
            }

            // Save to a MemoryStream
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            // Upload to your server/cloud storage and get the URL
            string fileName = $"{managerName}_AbsentReport_{DateTime.Now:yyyyMMdd}.xlsx";
            var execelformFile = new MemoryFormFile(stream, fileName);
            var folder = $"uploads/absent_report";
            var excelDownloadLink = await _fileUploadService.UploadAndReplaceDocumentAsync(execelformFile, folder,null);
            if (string.IsNullOrEmpty(excelDownloadLink))
            {
                excelDownloadLink = "#";
            }
            return excelDownloadLink;
        }

    }
}
