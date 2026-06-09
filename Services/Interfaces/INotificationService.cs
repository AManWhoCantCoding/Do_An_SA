namespace MediSphere.Services.Interfaces
{
    public interface INotificationService
    {
        Task SendAppointmentConfirmationAsync(string toEmail, string patientName, DateTime startTime, string topic);
        Task SendReportStatusUpdateAsync(string toEmail, string patientName, string reportStatus);
        Task SendWelcomeStaffNotificationAsync(string toEmail, string staffName);
    }
}
