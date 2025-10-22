using HRMS_Core.Services;
using HRMS_Core.VM.EmailService;
using HRMS_Infrastructure.Interface;
using HRMS_Infrastructure.Repository;
using System.Net;
using System.Net.Mail;

namespace HRMS_API.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _templateFolder;
        private readonly bool _enableEmail;
        private readonly string _environment;

        public EmailService(IConfiguration configuration, IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _templateFolder = _configuration["MailSettings:TemplateFolder"] ?? "EmailTemplates";
            _enableEmail = bool.Parse(_configuration["MailSettings:EnableEmail"] ?? "true");
            _environment = _configuration["MailSettings:Environment"] ?? "Production";
        }

        /// <summary>
        /// Sends an email using a template and dynamic placeholders.
        /// Returns true if successful, false if failed or disabled.
        /// </summary>
        public async Task<bool> SendEmailAsync(EmailRequest request)
        {
            try
            {
                if (!_enableEmail || _environment.Equals("Local", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("⚙️ Email sending skipped (Environment: Local or Disabled)");
                    return false;
                }

                // ✅ Read SMTP configuration
                string smtpHost = _configuration["MailSettings:Host"];
                int smtpPort = int.Parse(_configuration["MailSettings:Port"]);
                string fromEmail = _configuration["MailSettings:From"];
                string smtpUser = _configuration["MailSettings:UserName"];   // <-- important
                string smtpPass = _configuration["MailSettings:Password"];
                string senderName = _configuration["MailSettings:SenderName"] ?? "HRMS Notification";
                bool enableSsl = bool.Parse(_configuration["MailSettings:EnableSSL"] ?? "true");

                // ✅ Load template if provided
                string body = string.Empty;
                if (!string.IsNullOrEmpty(request.TemplateName))
                {
                    string templatePath = Path.Combine(Directory.GetCurrentDirectory(), _templateFolder, request.TemplateName);
                    if (!File.Exists(templatePath))
                        throw new FileNotFoundException($"Email template not found: {templatePath}");

                    body = await File.ReadAllTextAsync(templatePath);
                    body = ReplacePlaceholders(body, request.Placeholders);
                }

                using var smtp = new SmtpClient(smtpHost, smtpPort)
                {
                    EnableSsl = enableSsl,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(smtpUser, smtpPass) // ✅ use SMTP user, not from email
                };

                using var message = new MailMessage
                {
                    From = new MailAddress(fromEmail, senderName),
                    Subject = request.Subject,
                    Body = body,
                    IsBodyHtml = true
                };

                // Recipients
                if (request.ToEmails?.Any() == true)
                    foreach (var to in request.ToEmails.Distinct())
                        message.To.Add(to);

                if (request.CcEmails?.Any() == true)
                    foreach (var cc in request.CcEmails.Distinct())
                        message.CC.Add(cc);

                if (request.BccEmails?.Any() == true)
                    foreach (var bcc in request.BccEmails.Distinct())
                        message.Bcc.Add(bcc);

                // Attachments
                if (request.AttachmentPaths?.Any() == true)
                    foreach (var filePath in request.AttachmentPaths.Where(File.Exists))
                        message.Attachments.Add(new Attachment(filePath));

                await smtp.SendMailAsync(message);

                // ✅ Log the email as "Sent"
                var emailLogger = new EmailLogger
                {
                    FromEmail = fromEmail,
                    ToEmail = string.Join(";", request.ToEmails ?? new List<string>()),
                    CCEmail = string.Join(";", request.CcEmails ?? new List<string>()),
                    BCCEmail = string.Join(";", request.BccEmails ?? new List<string>()),
                    Subject = request.Subject,
                    Body = request.TemplateName,
                    Status = EmailStatus.Sent,
                    SentAt = DateTime.UtcNow,
                    AttachmentsUrl = string.Join(";", request.AttachmentPaths ?? new List<string>()),
                    Comments = "Email sent successfully",
                    CreatedAt = DateTime.UtcNow,
                };

                await _unitOfWork.EmailLoggerRepository.ManageEmailLoggerAsync(emailLogger, "CREATE");


                return true;
            }
            catch (Exception ex)
            {
                var emailLogger = new EmailLogger
                {
                    FromEmail = _configuration["MailSettings:From"],
                    ToEmail = string.Join(";", request.ToEmails ?? new List<string>()),
                    CCEmail = string.Join(";", request.CcEmails ?? new List<string>()),
                    BCCEmail = string.Join(";", request.BccEmails ?? new List<string>()),
                    Subject = request.Subject,
                    Body = request.TemplateName ?? "N/A",
                    Status = EmailStatus.Failed,
                    SentAt = DateTime.UtcNow,
                    AttachmentsUrl = string.Join(";", request.AttachmentPaths ?? new List<string>()),
                    Comments = $"Failed to send email: {ex.Message}"
                };

                await _unitOfWork.EmailLoggerRepository.ManageEmailLoggerAsync(emailLogger, "CREATE");

                return false;
            }
        }

        private string ReplacePlaceholders(string template, Dictionary<string, string>? placeholders)
        {
            if (placeholders == null || placeholders.Count == 0)
                return template;

            foreach (var kvp in placeholders)
                template = template.Replace($"{{{{{kvp.Key}}}}}", kvp.Value, StringComparison.OrdinalIgnoreCase);

            return template;
        }
    }
}
