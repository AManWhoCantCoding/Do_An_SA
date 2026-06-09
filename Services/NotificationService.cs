using MediSphere.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MediSphere.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailSender _emailSender;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IEmailSender emailSender, ILogger<NotificationService> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task SendAppointmentConfirmationAsync(string toEmail, string patientName, DateTime startTime, string topic)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                _logger.LogWarning("Skipped appointment confirmation — no email for patient {PatientName}.", patientName);
                return;
            }

            var subject = "MediSphere — Appointment Confirmation";
            var body = $"<p>Dear {patientName},</p>" +
                       "<p>Your appointment has been scheduled:</p>" +
                       $"<ul><li><strong>Topic:</strong> {topic}</li>" +
                       $"<li><strong>Date &amp; time:</strong> {startTime:g}</li></ul>" +
                       "<p>— MediSphere Hospital Management</p>";

            await _emailSender.SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendReportStatusUpdateAsync(string toEmail, string patientName, string reportStatus)
        {
            if (string.IsNullOrWhiteSpace(toEmail))
            {
                _logger.LogWarning("Skipped report status email — no email for patient {PatientName}.", patientName);
                return;
            }

            var subject = "MediSphere — Report Status Update";
            var body = $"<p>Dear {patientName},</p>" +
                       $"<p>Your medical report status has been updated to: <strong>{reportStatus}</strong>.</p>" +
                       "<p>— MediSphere Hospital Management</p>";

            await _emailSender.SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendWelcomeStaffNotificationAsync(string toEmail, string staffName)
        {
            var subject = "Welcome to MediSphere";
            var body = $"<p>Hello {staffName},</p>" +
                       "<p>Your staff account on MediSphere has been created. Please confirm your email to activate access.</p>" +
                       "<p>— MediSphere Hospital Management</p>";

            await _emailSender.SendEmailAsync(toEmail, subject, body);
        }
    }
}
