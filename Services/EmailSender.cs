using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace MediSphere.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;
        private readonly SmtpSettings _smtpSettings;

        public EmailSender(ILogger<EmailSender> logger, IOptions<SmtpSettings> smtpSettings)
        {
            _logger = logger;
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            if (!_smtpSettings.IsConfigured)
            {
                _logger.LogWarning(
                    "SMTP is not configured. Email to {Email} with subject '{Subject}' was not sent.",
                    toEmail,
                    subject);
                throw new InvalidOperationException(
                    "SMTP settings are not configured. Update SmtpSettings in appsettings.json.");
            }

            try
            {
                using var client = new SmtpClient(_smtpSettings.SmtpServer, _smtpSettings.SmtpPort)
                {
                    Credentials = new NetworkCredential(_smtpSettings.SmtpUsername, _smtpSettings.SmtpPassword),
                    EnableSsl = true
                };

                var fromAddress = _smtpSettings.FromEmail ?? _smtpSettings.SmtpUsername;
                var from = string.IsNullOrWhiteSpace(_smtpSettings.FromDisplayName)
                    ? new MailAddress(fromAddress)
                    : new MailAddress(fromAddress, _smtpSettings.FromDisplayName);

                using var mailMessage = new MailMessage
                {
                    From = from,
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(toEmail);

                await client.SendMailAsync(mailMessage);

                _logger.LogInformation("Email to {Email} sent successfully.", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}.", toEmail);
                throw;
            }
        }
    }
}
