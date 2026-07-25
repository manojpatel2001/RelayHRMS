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

        /// <summary>
        /// Hangfire will call this method for recurring emails (Reporting Manager wise)
        /// </summary>
        public async Task SendReportingDailyEmailAsync()
        {
            using (JobStorage.Current.GetConnection().AcquireDistributedLock("reporting-email-lock", TimeSpan.FromMinutes(5)))
            {

                try
                {
                    var emailReport = await _unitOfWork.EmailReportRepository.GetEmailSendTime(EmailReportType.DailyAbsentEmployeesReport.ToString());

                    if (emailReport == null
                        || emailReport.EmailSendTime == null
                        || emailReport.ServerType?.ToUpper() != "LIVE")
                    {
                        Console.WriteLine("⛔ Reporting email skipped");
                        return;
                    }
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
        }

        /// <summary>
        /// Sends combined absent report to HR only (all managers + employees in one email)
        /// </summary>
        public async Task SendHrDailyEmailAsync()
        {
            
            using (JobStorage.Current.GetConnection().AcquireDistributedLock("hr-email-lock", TimeSpan.FromMinutes(5)))
            {
                try
                {

                    var allEmailReport = await _unitOfWork.EmailReportRepository.GetEmailSendTime(EmailReportType.DailyAbsentAllEmployeesReport.ToString());

                    // ✅ NULL + SERVER CHECK
                    if (allEmailReport == null
                        || allEmailReport.EmailSendTime == null
                        || allEmailReport.ServerType?.ToUpper() != "LIVE")
                    {
                        Console.WriteLine("⛔ Scheduler skipped (Not LIVE or invalid data)");

                        return;
                    }

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
                    Console.WriteLine($"❌ Error in SendHrDailyEmailAsync: {ex.Message}");
                    Console.WriteLine(ex.StackTrace);
                }
            }
        }

        /// <summary>
        /// Sends daily left employee report email
        /// </summary>
        public async Task SendDailyLeftEmployeeEmailAsync()
        {
            using (JobStorage.Current.GetConnection().AcquireDistributedLock("left-employee-email-lock", TimeSpan.FromMinutes(5)))
            {
                try
                {
                    var emailReport = await _unitOfWork.EmailReportRepository
                   .GetEmailSendTime(EmailReportType.DailyLeftEmployeeReport.ToString());

                    if (emailReport == null
                        || emailReport.EmailSendTime == null
                        || emailReport.ServerType?.ToUpper() != "LIVE")
                    {
                        Console.WriteLine("⛔ Left Employee email skipped");
                        return;
                    }
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
        }


        // =====================================================================
        //   SCHEDULE METHODS — FIX APPLIED: Only re-register if cron changed
        // =====================================================================

        /// <summary>
        /// ✅ FIXED: Only updates Hangfire job if send time has changed in DB.
        /// Prevents double-firing caused by Remove + AddOrUpdate near execution time.
        /// </summary>
        public async Task ScheduleReportingDailyEmail()
        {
            try
            {
                var emailReport = await _unitOfWork.EmailReportRepository
                    .GetEmailSendTime(EmailReportType.DailyAbsentEmployeesReport.ToString());

                // ✅ NULL + SERVER CHECK
                if (emailReport == null
                    || emailReport.EmailSendTime == null
                    || emailReport.ServerType?.ToUpper() != "LIVE")
                {
                    Console.WriteLine("⛔ Reporting scheduler skipped (Not LIVE or invalid)");

                    RecurringJob.RemoveIfExists("reporting-daily-email");
                    return;
                }

                var istTime = emailReport.EmailSendTime.Value;

                // ✅ IST TimeZone
                var istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                // ✅ Get current IST date (IMPORTANT 🔥)
                var istNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istZone);

                // ✅ Create IST DateTime correctly
                var istDateTime = new DateTime(
                    istNow.Year,
                    istNow.Month,
                    istNow.Day,
                    istTime.Hours,
                    istTime.Minutes,
                    0,
                    DateTimeKind.Unspecified
                );

                // ✅ Convert IST → UTC
                var utcTime = TimeZoneInfo.ConvertTimeToUtc(istDateTime, istZone);

                // ✅ Create CRON (UTC)
                string newCron = $"{utcTime.Minute} {utcTime.Hour} * * *";

                // ✅ CHECK EXISTING JOB
                var existingJob = JobStorage.Current
                    .GetConnection()
                    .GetRecurringJobs()
                    .FirstOrDefault(j => j.Id == "reporting-daily-email");

                if (existingJob != null && existingJob.Cron == newCron)
                {
                    Console.WriteLine("⏭️ Reporting job unchanged. Skipping.");
                    return;
                }

                // ✅ Schedule job
                RecurringJob.AddOrUpdate(
                    "reporting-daily-email",
                    () => SendReportingDailyEmailAsync(),
                    newCron
                );

                Console.WriteLine($"✅ Reporting job scheduled | IST: {istTime} | UTC: {utcTime}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error scheduling reporting job: {ex.Message}");
            }
        }
        /// <summary>
        /// ✅ FIXED: Only updates Hangfire job if send time has changed in DB.
        /// Prevents HR email from firing twice at scheduled time.
        /// </summary>
        public async Task ScheduleHrDailyEmail()
        {
            try
            {
                var emailReport = await _unitOfWork.EmailReportRepository
                    .GetEmailSendTime(EmailReportType.DailyAbsentAllEmployeesReport.ToString());

                // ✅ NULL + SERVER CHECK
                if (emailReport == null
                    || emailReport.EmailSendTime == null
                    || emailReport.ServerType?.ToUpper() != "LIVE")
                {
                    Console.WriteLine("⛔ Scheduler skipped");

                    RecurringJob.RemoveIfExists("hr-daily-email");
                    return;
                }

                var istTime = emailReport.EmailSendTime.Value;

                // ✅ IST TimeZone
                var istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                // ✅ Get today's IST date
                var istNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istZone);

                // ✅ Create IST datetime properly
                var istDateTime = new DateTime(
                    istNow.Year,
                    istNow.Month,
                    istNow.Day,
                    istTime.Hours,
                    istTime.Minutes,
                    0,
                    DateTimeKind.Unspecified
                );

                // ✅ Convert IST → UTC
                var utcTime = TimeZoneInfo.ConvertTimeToUtc(istDateTime, istZone);

                // ✅ Create CRON in UTC
                string newCron = $"{utcTime.Minute} {utcTime.Hour} * * *";

                // ✅ CHECK EXISTING JOB
                var existingJob = JobStorage.Current
                    .GetConnection()
                    .GetRecurringJobs()
                    .FirstOrDefault(j => j.Id == "hr-daily-email");

                if (existingJob != null && existingJob.Cron == newCron)
                {
                    Console.WriteLine("⏭️ No change in schedule");
                    return;
                }

                // ✅ Schedule in UTC (Hangfire default)
                RecurringJob.AddOrUpdate(
                    "hr-daily-email",
                    () => SendHrDailyEmailAsync(),
                    newCron
                );

                Console.WriteLine($"✅ Scheduled IST: {istTime} | UTC: {utcTime}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }


        public void ScheduleDaily7AMJob()
        {
            try
            {
                // IST → UTC conversion (7:00 AM IST)
                var istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                var istNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istZone);

                var istDateTime = new DateTime(
                    istNow.Year,
                    istNow.Month,
                    istNow.Day,
                    7,   // Hour = 7 AM
                    0,
                    0,
                    DateTimeKind.Unspecified
                );

                var utcTime = TimeZoneInfo.ConvertTimeToUtc(istDateTime, istZone);

                string cron = $"{utcTime.Minute} {utcTime.Hour} * * *";

                RecurringJob.AddOrUpdate(
                    "daily-7am-job",
                    () => RunDailyTask(),
                    cron
                );

                Console.WriteLine($"✅ Job scheduled at 7 AM IST (UTC: {utcTime})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }

        public async Task RunDailyTask()
        {
            // 🔥 Call your SP here
            await _unitOfWork.EmailReportRepository.InsertAbsentEmployeeEmailData();

            Console.WriteLine("✅ Daily task executed at 7 AM");
        }
        /// <summary>
        /// ✅ FIXED: Only updates Hangfire job if send time has changed in DB.
        /// Prevents left employee email from firing twice.
        /// </summary>
        public async Task ScheduleDailyLeftEmployeeEmail()
        {
            try
            {
                var emailReport = await _unitOfWork.EmailReportRepository
                    .GetEmailSendTime(EmailReportType.DailyLeftEmployeeReport.ToString());

                // ✅ NULL + SERVER CHECK
                if (emailReport == null
                    || emailReport.EmailSendTime == null
                    || emailReport.ServerType?.ToUpper() != "LIVE")
                {
                    Console.WriteLine("⛔ Left Employee scheduler skipped (Not LIVE or invalid)");

                    RecurringJob.RemoveIfExists("daily-left-employee-email");
                    return;
                }

                var istTime = emailReport.EmailSendTime.Value;

                // ✅ IST TimeZone
                var istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

                // ✅ Get current IST date (IMPORTANT 🔥)
                var istNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istZone);

                // ✅ Create IST DateTime properly
                var istDateTime = new DateTime(
                    istNow.Year,
                    istNow.Month,
                    istNow.Day,
                    istTime.Hours,
                    istTime.Minutes,
                    0,
                    DateTimeKind.Unspecified
                );

                // ✅ Convert IST → UTC
                var utcTime = TimeZoneInfo.ConvertTimeToUtc(istDateTime, istZone);

                // ✅ Create CRON (UTC)
                string newCron = $"{utcTime.Minute} {utcTime.Hour} * * *";

                // ✅ CHECK EXISTING JOB
                var existingJob = JobStorage.Current
                    .GetConnection()
                    .GetRecurringJobs()
                    .FirstOrDefault(j => j.Id == "daily-left-employee-email");

                if (existingJob != null && existingJob.Cron == newCron)
                {
                    Console.WriteLine("⏭️ Left Employee job unchanged. Skipping.");
                    return;
                }

                // ✅ Schedule job
                RecurringJob.AddOrUpdate(
                    "daily-left-employee-email",
                    () => SendDailyLeftEmployeeEmailAsync(),
                    newCron
                );

                Console.WriteLine($"✅ Left Employee job scheduled | IST: {istTime} | UTC: {utcTime}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error scheduling Left Employee job: {ex.Message}");
            }
        }
        /// <summary>
        /// Registers the 3 schedule-checker jobs (run every 2 min to detect DB time changes).
        /// ✅ Safe now because each Schedule* method only re-registers when cron actually changes.
        /// </summary>
        public void StartScheduleDailyEmail()
        {
            RecurringJob.RemoveIfExists("reporting-schedule-check");
            RecurringJob.RemoveIfExists("hr-schedule-check");
            RecurringJob.RemoveIfExists("left-employee-schedule-check");

            RecurringJob.AddOrUpdate(
                "reporting-schedule-check",
                () => ScheduleReportingDailyEmail(),n
                "*/2 * * * *"
            );

            RecurringJob.AddOrUpdate(
               "hr-schedule-check",
               () => ScheduleHrDailyEmail(),
               "*/2 * * * *"
           );

            RecurringJob.AddOrUpdate(
                "left-employee-schedule-check",
                () => ScheduleDailyLeftEmployeeEmail(),
                "*/2 * * * *"
            );

            ScheduleDaily7AMJob();
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