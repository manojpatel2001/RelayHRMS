using ClosedXML.Excel;
using DocumentFormat.OpenXml.ExtendedProperties;
using Hangfire;
using HRMS_Core.Services;
using HRMS_Core.VM.EmailService;
using HRMS_Infrastructure.Interface;
using HRMS_Utility;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Engineering;
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
        public async Task SendReportingDailyEmailAsync(EmailReport? emailReport)
        {
            try
            {
                
                List<DailyAbsentReportResult>? AbsentReport = await _unitOfWork.EmailReportRepository.GetDailyAbsentReport();

               
                if (emailReport==null  || AbsentReport == null|| !AbsentReport.Any())
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
                    if (emailReport != null)
                    {
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
                        var ToEmails = "";
                        if (!string.IsNullOrEmpty(emailReport.ToEmails) && !string.IsNullOrEmpty(manager.ReportingManagerEmail))
                        {
                            ToEmails = $"{manager.ReportingManagerEmail},{emailReport.ToEmails}";
                        }
                        else if (!string.IsNullOrEmpty(emailReport.ToEmails))
                        {
                            ToEmails = $"{emailReport.ToEmails}";
                        }
                        else if (!string.IsNullOrEmpty(manager.ReportingManagerEmail))
                        {
                            ToEmails = $"{manager.ReportingManagerEmail}";
                        }

                        if (string.IsNullOrEmpty(ToEmails))
                        {

                            continue;
                        }

                        var emailRequest = new EmailRequest
                        {
                            ToEmails = ToEmails.Split(',').ToList(),
                            CcEmails = emailReport?.CcEmails?.Split(',').ToList(),
                            BccEmails = emailReport?.BccEmails?.Split(',').ToList(),
                            Subject = $"{emailReport.Subject} – {dateFormate}",
                            TemplateName = "DailyAbsentReportEmailTemplate.html",
                            Placeholders = placeholders,
                            AttachmentPaths = excelDownloadLink
                        };


                        var reportingEmailLogger = new EmailLogger
                        {
                            ToEmail = ToEmails,
                            CCEmail = emailReport?.CcEmails,
                            BCCEmail = emailReport?.BccEmails,
                            Subject = emailRequest.Subject,
                            Body = emailRequest.TemplateName,
                            Status = EmailStatus.Pending,
                            SentAt = DateTime.UtcNow,
                            AttachmentsUrl = excelDownloadLink,
                            Comments = "Email ready for sent"

                        };

                        await _unitOfWork.EmailLoggerRepository.ManageEmailLoggerAsync(reportingEmailLogger, "CREATE");

                        //Send the email
                        bool result = await _emailService.SendEmailAsync(emailRequest);
                        if (result)
                            Console.WriteLine($"✅ Daily Absentee email sent to {manager.ReportingManagerName}.");
                        else
                            Console.WriteLine($"⚠️ Email sending failed for {manager.ReportingManagerName}.");
                    }
                }
              
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in SendDailyEmailAsync: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
        public async Task SendHrDailyEmailAsync(EmailReport? allEmailReport)
        {
            try
            {
                
                List<DailyAbsentReportResult>? AbsentReport = await _unitOfWork.EmailReportRepository.GetDailyAbsentReport();

               
                
                if (allEmailReport==null || AbsentReport == null|| !AbsentReport.Any())
                    return;

                var allEmployeeRows = new StringBuilder();
                string allExcelDownloadLink = await GenerateAndUploadAllExcel(AbsentReport);

                // Iterate over each reporting manager
                foreach (var manager in AbsentReport)
                {
                         allEmployeeRows.AppendLine($@"
                         <tr>  <td colspan='5' style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:#f8f9fa;'><b>Reporting Persion:</b> {manager.ReportingManagerName}</td>
							</tr>

                         ");
                    // Build dynamic employee table rows for this manager's team
                    int rowNum = 0;
                    int i = 1;
                    foreach (var emp in manager.Employees)
                    {
                        rowNum++;
                        var backgroundColor = rowNum % 2 == 0 ? "#f8f9fa" : "#ffffff";
                        

                        allEmployeeRows.AppendLine($@"
                           <tr>
                             <td style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:{backgroundColor};'>{i}</td>
                            <td style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:{backgroundColor};'>{emp.EmployeeName}</td>
                            <td style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:{backgroundColor};'>{emp.EmployeeCode}</td>
                            <td style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:{backgroundColor};'>{emp.BranchName}</td>
                            <td style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:{backgroundColor};'>{emp.Attendance}</td>
                         </tr>

                         ");
                        i++;
                    }
                    

                }
                allEmployeeRows.AppendLine($@"
                    <tr>
                        <td colspan='5' style='padding:10px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:#e9ecef;'>
                            <a href='{allExcelDownloadLink}' style='color: #0066cc; text-decoration: none;'>Download Absentee Report (Excel)</a>
                                <p style=""margin: 0 0 10px 0; color: #666666; font-size: 12px; line-height: 1.5;"">
                                    The Absentee Report (Excel) will be available for download for up to 10 days from the report generation date..
                                </p>
                        </td>
                    </tr>");

                if (allEmailReport != null)
                {

                    var AllDateFormate = DateTime.Now.AddDays(-1).ToString("dd MMM yyyy");
                    var AllPlaceholders = new Dictionary<string, string>
                    {
                        { "Date", AllDateFormate },
                        { "HRContactNumber", allEmailReport.HRContactNumber??"" },
                        { "HRContactEmail",  allEmailReport.HRContactEmail??"" },
                        { "AllEmployeeRows", allEmployeeRows.ToString() }
                    };

                    // Prepare email request object
                    var AllToEmails = $"{allEmailReport.ToEmails}";


                    if (string.IsNullOrEmpty(AllToEmails))
                    {
                        return;
                    }

                    var AllEmailRequest = new EmailRequest
                    {
                        ToEmails = AllToEmails.Split(',').ToList(),
                        CcEmails = allEmailReport?.CcEmails?.Split(',').ToList(),
                        BccEmails = allEmailReport?.BccEmails?.Split(',').ToList(),
                        Subject = $"{allEmailReport.Subject} – {AllDateFormate}",
                        TemplateName = "DailyAbsentAllReportEmailTemplate.html",
                        Placeholders = AllPlaceholders,
                        AttachmentPaths = allExcelDownloadLink
                    };



                    var allEmailLogger = new EmailLogger
                    {
                        ToEmail = AllToEmails,
                        CCEmail = allEmailReport?.CcEmails,
                        BCCEmail = allEmailReport?.BccEmails,
                        Subject = AllEmailRequest.Subject,
                        Body = AllEmailRequest.TemplateName,
                        Status = EmailStatus.Pending,
                        SentAt = DateTime.UtcNow,
                        AttachmentsUrl = allExcelDownloadLink,
                        Comments = "Email ready for sent"

                    };

                    await _unitOfWork.EmailLoggerRepository.ManageEmailLoggerAsync(allEmailLogger, "CREATE");

                    //Send the email
                    bool allResult = await _emailService.SendEmailAsync(AllEmailRequest);
                    if (allResult)
                        Console.WriteLine($"✅ Daily Absentee email sent to {allEmailReport.ToEmails}.");
                    else
                        Console.WriteLine($"⚠️ Email sending failed for {allEmailReport.ToEmails}.");
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
        public async Task ScheduleReportingDailyEmail()
        {
            try
            {
                var emailReport = await _unitOfWork.EmailReportRepository
                    .GetEmailSendTime(EmailReportType.DailyAbsentEmployeesReport.ToString());

                if (emailReport?.EmailSendTime == null)
                    return;

                string cronExpression = $"{emailReport.EmailSendTime.Value.Minutes} {emailReport.EmailSendTime.Value.Hours} * * *";

                RecurringJob.RemoveIfExists("reporting-daily-email");
                RecurringJob.AddOrUpdate(
                    "reporting-daily-email",
                    () => SendReportingDailyEmailAsync(emailReport),
                    cronExpression
                );

                Console.WriteLine("✅ Reporting daily email job scheduled successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error scheduling reporting email job: {ex.Message}");
            }
        }

        public async Task ScheduleHrDailyEmail()
        {
            try
            {
                var emailReport = await _unitOfWork.EmailReportRepository
                    .GetEmailSendTime(EmailReportType.DailyAbsentAllEmployeesReport.ToString());

                if (emailReport?.EmailSendTime == null)
                    return;

                string cronExpression = $"{emailReport.EmailSendTime.Value.Minutes} {emailReport.EmailSendTime.Value.Hours} * * *";

                RecurringJob.RemoveIfExists("hr-daily-email");
                RecurringJob.AddOrUpdate(
                    "hr-daily-email",
                    () => SendHrDailyEmailAsync(emailReport),
                    cronExpression
                );

                Console.WriteLine("✅ HR daily email job scheduled successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error scheduling HR email job: {ex.Message}");
            }
        }


        public void StartScheduleDailyEmail()
        {
            RecurringJob.AddOrUpdate(
                "reporting-schedule-check",
                () => ScheduleReportingDailyEmail(),
                "*/2 * * * *"
            );

            RecurringJob.AddOrUpdate(
                "hr-schedule-check",
                () => ScheduleHrDailyEmail(),
                "*/2 * * * *"
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
        private async Task<string> GenerateAndUploadAllExcel(List<DailyAbsentReportResult> allEmployees)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Absent Employees");

            // Add headers
            worksheet.Cell(1, 1).Value = "S.No";
            worksheet.Cell(1, 2).Value = "Employee Name";
            worksheet.Cell(1, 3).Value = "Employee Code";
            worksheet.Cell(1, 4).Value = "Branch";
            worksheet.Cell(1, 5).Value = "Attendance";

            int row = 2; // Start from row 2 (row 1 is headers)

            foreach (var manager in allEmployees)
            {
                // Add Reporting Manager as a header row
                worksheet.Cell(row, 1).Value = $"Reporting Person: {manager.ReportingManagerName}";
                worksheet.Range(row, 1, row, 5).Merge().Style.Font.Bold = true;
                worksheet.Range(row, 1, row, 5).Style.Fill.BackgroundColor = XLColor.LightGray;
                row++;

                // Add employees under the manager
                for (int i = 0; i < manager.Employees.Count; i++)
                {
                    var emp = manager.Employees[i];
                    worksheet.Cell(row, 1).Value = i + 1; // S.No
                    worksheet.Cell(row, 2).Value = emp.EmployeeName;
                    worksheet.Cell(row, 3).Value = emp.EmployeeCode;
                    worksheet.Cell(row, 4).Value = emp.BranchName;
                    worksheet.Cell(row, 5).Value = emp.Attendance;
                    row++;
                }

                // Add a blank row after each manager's employees for clarity
                row++;
            }

            // Auto-fit columns for better readability
            worksheet.Columns().AdjustToContents();

            // Save to a MemoryStream
            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            // Upload to your server/cloud storage and get the URL
            string fileName = $"All_AbsentReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            var excelFormFile = new MemoryFormFile(stream, fileName);
            var folder = $"uploads/absent_report";
            var excelDownloadLink = await _fileUploadService.UploadAndReplaceDocumentAsync(excelFormFile, folder, null);

            return string.IsNullOrEmpty(excelDownloadLink) ? "#" : excelDownloadLink;
        }

    }
}
