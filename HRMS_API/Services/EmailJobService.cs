using ClosedXML.Excel;
using DocumentFormat.OpenXml.ExtendedProperties;
using Hangfire;
using Hangfire.Storage;
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


        public async Task ProcessJobsAdvanced()
        {
            var now = DateTime.UtcNow;

            Console.WriteLine($"🕒 Checking at {now}");

            // 🔥 Prepare Data
            await RefreshPrepareEmailData();

            List<EmailAllReport> reports = await _unitOfWork.EmailReportRepository.GetAllEmailSendTime();

            foreach (var report in reports)
            {
                if (report.EmailSendTime == null)
                    continue;

                var time = report.EmailSendTime.Value;

                // ✅ Time match (tolerance 1 min)
                bool isMatch = time.Hours == now.Hour &&
                               Math.Abs(time.Minutes - now.Minute) <= 1;

                if (!isMatch)
                    continue;

                //// 🚫 Duplicate prevent
             
                if (report.LastRunDate == now.Date && !report.IsForceSend)
                {
                    continue;
                }

                Console.WriteLine($"🔥 Running: {report.ReportName}");

                bool success = false;
                int retry = 0;

                while (!success && retry < 3)
                {
                    try
                    {
                        switch (report.ReportName)
                        {
                            case "Daily Absent Employees Report":
                                await SendReportingEmailAsync(report);
                                break;

                            case "Daily Absent All Employees Report":
                                await SendHrEmailAsync(report);
                                break;

                            case "Daily Left Employee Report":
                                await SendLeftEmployeeEmailAsync(report);
                                break;
                        }

                        success = true;

                        report.LastRunDate = now;
                        report.IsSuccess = true;

                        await _unitOfWork.EmailReportRepository.UpdateEmailReport(report);

                        Console.WriteLine($"✅ Success: {report.ReportName}");
                    }
                    catch (Exception ex)
                    {
                        retry++;

                        Console.WriteLine($"⚠️ Retry {retry}: {ex.Message}");

                        if (retry == 3)
                        {
                            report.IsSuccess = false;
                            report.LastError = ex.Message;

                             await _unitOfWork.EmailReportRepository.UpdateEmailReport(report);

                            Console.WriteLine($"❌ Failed: {report.ReportName}");
                        }

                        await Task.Delay(2000);
                    }
                }
            }
        }

        /// <summary>
        /// Hangfire will call this method for recurring emails (Reporting Manager wise)
        /// </summary>
        public async Task SendReportingEmailAsync(EmailAllReport? emailReport)
        {
            try
            {
                List<DailyAbsentReportResult>? AbsentReport = await _unitOfWork.EmailReportRepository.GetEmployeeEmailDataGrouped();

                if (emailReport == null || AbsentReport == null || !AbsentReport.Any())
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
                                The Absentee Report (Excel) will be available for download for up to 10 days from the report generation date.
                            </p>
                        </td>
                    </tr>");

                    if (emailReport != null)
                    {
                        var dateFormate = DateTime.Now.AddDays(-1).ToString("dd MMM yyyy");
                        var placeholders = new Dictionary<string, string>
                        {
                            { "Date", dateFormate },
                            { "ManagerName", manager.ReportingManagerName ?? "" },
                            { "HRContactNumber", emailReport.HRContactNumber ?? "" },
                            { "HRContactEmail", emailReport.HRContactEmail ?? "" },
                            { "EmployeeRows", employeeRows.ToString() }
                        };

                        var ToEmails = "";
                        if (!string.IsNullOrEmpty(emailReport.ToEmails) && !string.IsNullOrEmpty(manager.ReportingManagerEmail))
                            ToEmails = $"{manager.ReportingManagerEmail},{emailReport.ToEmails}";
                        else if (!string.IsNullOrEmpty(emailReport.ToEmails))
                            ToEmails = $"{emailReport.ToEmails}";
                        else if (!string.IsNullOrEmpty(manager.ReportingManagerEmail))
                            ToEmails = $"{manager.ReportingManagerEmail}";

                        if (string.IsNullOrEmpty(ToEmails))
                            continue;

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
                Console.WriteLine($"❌ Error in SendReportingDailyEmailAsync: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Sends combined absent report to HR only (all managers + employees in one email)
        /// </summary>
        public async Task SendHrEmailAsync(EmailAllReport? allEmailReport)
        {
            try
            {
                List<DailyAbsentReportResult>? AbsentReport = await _unitOfWork.EmailReportRepository.GetEmployeeEmailDataGrouped();

                if (allEmailReport == null || AbsentReport == null || !AbsentReport.Any())
                    return;

                var allEmployeeRows = new StringBuilder();
                string allExcelDownloadLink = await GenerateAndUploadAllExcel(AbsentReport);

                foreach (var manager in AbsentReport)
                {
                    allEmployeeRows.AppendLine($@"
                    <tr>
                        <td colspan='5' style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:#f8f9fa;'>
                            <b>Reporting Person:</b> {manager.ReportingManagerName}
                        </td>
                    </tr>");

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
                        </tr>");
                        i++;
                    }
                }

                allEmployeeRows.AppendLine($@"
                <tr>
                    <td colspan='5' style='padding:10px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:#e9ecef;'>
                        <a href='{allExcelDownloadLink}' style='color: #0066cc; text-decoration: none;'>Download Absentee Report (Excel)</a>
                        <p style=""margin: 0 0 10px 0; color: #666666; font-size: 12px; line-height: 1.5;"">
                            The Absentee Report (Excel) will be available for download for up to 10 days from the report generation date.
                        </p>
                    </td>
                </tr>");

                if (allEmailReport != null)
                {
                    var AllDateFormate = DateTime.Now.AddDays(-1).ToString("dd MMM yyyy");
                    var AllPlaceholders = new Dictionary<string, string>
                    {
                        { "Date", AllDateFormate },
                        { "HRContactNumber", allEmailReport.HRContactNumber ?? "" },
                        { "HRContactEmail", allEmailReport.HRContactEmail ?? "" },
                        { "AllEmployeeRows", allEmployeeRows.ToString() }
                    };

                    var AllToEmails = $"{allEmailReport.ToEmails}";

                    if (string.IsNullOrEmpty(AllToEmails))
                        return;
                    
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

                    bool allResult = await _emailService.SendEmailAsync(AllEmailRequest);
                    if (allResult)
                        Console.WriteLine($"✅ Daily Absentee email sent to {allEmailReport.ToEmails}.");
                    else
                        Console.WriteLine($"⚠️ Email sending failed for {allEmailReport.ToEmails}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in SendHrEmailAsync: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }




        /// <summary>
        /// Sends daily left employee report email
        /// </summary>
        public async Task SendLeftEmployeeEmailAsync(EmailAllReport? emailReport)
        {
            try
            {
                List<TodayLeftEmployeeEmailVM> leftEmployees = await _unitOfWork.EmailReportRepository.GetTodayLeftEmployeesEmailData();

                if (emailReport == null || leftEmployees == null || !leftEmployees.Any())
                    return;

                var employeeRows = new StringBuilder();
                int rowNum = 0;
                foreach (var emp in leftEmployees)
                {
                    rowNum++;
                    var backgroundColor = rowNum % 2 == 0 ? "#f8f9fa" : "#ffffff";
                    employeeRows.AppendLine($@"
                <tr>
                    <td style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:{backgroundColor};'>{emp.SerialNo}</td>
                    <td style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:{backgroundColor};'>{emp.Name}</td>
                    <td style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:{backgroundColor};'>{emp.EmployeeCode}</td>
                    <td style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:{backgroundColor};'>{emp.BranchName}</td>
                    <td style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:{backgroundColor};'>{emp.LeftDate}</td>
                    <td style='padding:6px 8px;font-size:13px;color:#333;border:1px solid #dee2e6;background-color:{backgroundColor};'>{emp.LeftEnteredOn}</td>
                </tr>");
                }

                var dateFormat = DateTime.Now.ToString("dd MMM yyyy");
                var placeholders = new Dictionary<string, string>
                {
                    { "Date", dateFormat },
                    { "ManagerName", "HR Manager" },
                    { "HRContactNumber", emailReport.HRContactNumber ?? "" },
                    { "HRContactEmail", emailReport.HRContactEmail ?? "" },
                    { "EmployeeRows", employeeRows.ToString() }
                };

                if (string.IsNullOrEmpty(emailReport.ToEmails))
                    return;


                var emailRequest = new EmailRequest
                {
                    ToEmails = emailReport.ToEmails.Split(',').ToList(),
                    CcEmails = emailReport?.CcEmails?.Split(',').ToList(),
                    BccEmails = emailReport?.BccEmails?.Split(',').ToList(),
                    Subject = $"{emailReport.Subject} – {dateFormat}",
                    TemplateName = "DailyLeftEmployeeReportEmailTemplate.html",
                    Placeholders = placeholders
                };

                var leftEmployeeEmailLogger = new EmailLogger
                {
                    ToEmail = emailReport.ToEmails,
                    CCEmail = emailReport?.CcEmails,
                    BCCEmail = emailReport?.BccEmails,
                    Subject = emailRequest.Subject,
                    Body = emailRequest.TemplateName,
                    Status = EmailStatus.Pending,
                    SentAt = DateTime.UtcNow,
                    Comments = "Email ready for sent"
                };

                await _unitOfWork.EmailLoggerRepository.ManageEmailLoggerAsync(leftEmployeeEmailLogger, "CREATE");

                bool result = await _emailService.SendEmailAsync(emailRequest);
                if (result)
                    Console.WriteLine($"✅ Daily Left Employee email sent successfully.");
                else
                    Console.WriteLine($"⚠️ Email sending failed for Daily Left Employee Report.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in SendDailyLeftEmployeeEmailAsync: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }


        public async Task RefreshPrepareEmailData()
        {
            Console.WriteLine("🔥 Preparing email data...");

            await _unitOfWork.EmailReportRepository.InsertAbsentEmployeeEmailData();
        }

        // =====================================================================
        //   EXCEL GENERATION HELPERS
        // =====================================================================

        private async Task<string> GenerateAndUploadExcel(List<EmployeeRecord> employees, string managerName)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Absent Employees");

            worksheet.Cell(1, 1).Value = "S.No";
            worksheet.Cell(1, 2).Value = "Employee Name";
            worksheet.Cell(1, 3).Value = "Employee Code";
            worksheet.Cell(1, 4).Value = "Branch";
            worksheet.Cell(1, 5).Value = "Attendance";

            for (int i = 0; i < employees.Count; i++)
            {
                var emp = employees[i];
                worksheet.Cell(i + 2, 1).Value = i + 1;
                worksheet.Cell(i + 2, 2).Value = emp.EmployeeName;
                worksheet.Cell(i + 2, 3).Value = emp.EmployeeCode;
                worksheet.Cell(i + 2, 4).Value = emp.BranchName;
                worksheet.Cell(i + 2, 5).Value = emp.Attendance;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            string fileName = $"{managerName}_AbsentReport_{DateTime.Now:yyyyMMdd}.xlsx";
            var excelFormFile = new MemoryFormFile(stream, fileName);
            var folder = $"uploads/absent_report";
            var excelDownloadLink = await _fileUploadService.UploadAndReplaceDocumentAsync(excelFormFile, folder, null);

            return string.IsNullOrEmpty(excelDownloadLink) ? "#" : excelDownloadLink;
        }

        private async Task<string> GenerateAndUploadAllExcel(List<DailyAbsentReportResult> allEmployees)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Absent Employees");

            worksheet.Cell(1, 1).Value = "S.No";
            worksheet.Cell(1, 2).Value = "Employee Name";
            worksheet.Cell(1, 3).Value = "Employee Code";
            worksheet.Cell(1, 4).Value = "Branch";
            worksheet.Cell(1, 5).Value = "Attendance";

            int row = 2;

            foreach (var manager in allEmployees)
            {
                worksheet.Cell(row, 1).Value = $"Reporting Person: {manager.ReportingManagerName}";
                worksheet.Range(row, 1, row, 5).Merge().Style.Font.Bold = true;
                worksheet.Range(row, 1, row, 5).Style.Fill.BackgroundColor = XLColor.LightGray;
                row++;

                for (int i = 0; i < manager.Employees.Count; i++)
                {
                    var emp = manager.Employees[i];
                    worksheet.Cell(row, 1).Value = i + 1;
                    worksheet.Cell(row, 2).Value = emp.EmployeeName;
                    worksheet.Cell(row, 3).Value = emp.EmployeeCode;
                    worksheet.Cell(row, 4).Value = emp.BranchName;
                    worksheet.Cell(row, 5).Value = emp.Attendance;
                    row++;
                }

                row++; // blank row after each manager
            }

            worksheet.Columns().AdjustToContents();

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            string fileName = $"All_AbsentReport_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            var excelFormFile = new MemoryFormFile(stream, fileName);
            var folder = $"uploads/absent_report";
            var excelDownloadLink = await _fileUploadService.UploadAndReplaceDocumentAsync(excelFormFile, folder, null);

            return string.IsNullOrEmpty(excelDownloadLink) ? "#" : excelDownloadLink;
        }

        private async Task<string> GenerateAndUploadLeftEmployeeExcel(List<TodayLeftEmployeeEmailVM> leftEmployees, string reportName)
        {
            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Left Employees");

            worksheet.Cell(1, 1).Value = "S.No";
            worksheet.Cell(1, 2).Value = "Employee Name";
            worksheet.Cell(1, 3).Value = "Employee Code";
            worksheet.Cell(1, 4).Value = "Branch";
            worksheet.Cell(1, 5).Value = "Left Date";

            for (int i = 0; i < leftEmployees.Count; i++)
            {
                var emp = leftEmployees[i];
                worksheet.Cell(i + 2, 1).Value = i + 1;
                worksheet.Cell(i + 2, 2).Value = emp.Name;
                worksheet.Cell(i + 2, 3).Value = emp.EmployeeCode;
                worksheet.Cell(i + 2, 4).Value = emp.BranchName;
                worksheet.Cell(i + 2, 5).Value = emp.LeftDate;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Position = 0;

            string fileName = $"{reportName}_LeftEmployeeReport_{DateTime.Now:yyyyMMdd}.xlsx";
            var excelFormFile = new MemoryFormFile(stream, fileName);
            var folder = $"uploads/left_employee_report";
            var excelDownloadLink = await _fileUploadService.UploadAndReplaceDocumentAsync(excelFormFile, folder, null);

            return string.IsNullOrEmpty(excelDownloadLink) ? "#" : excelDownloadLink;
        }
    }
}