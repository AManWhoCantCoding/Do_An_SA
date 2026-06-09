namespace MediSphere
{
    public class SmtpSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = 587;
        public string SmtpUsername { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public string? FromEmail { get; set; }
        public string? FromDisplayName { get; set; }

        public bool IsConfigured =>
            !string.IsNullOrWhiteSpace(SmtpServer) &&
            SmtpPort > 0 &&
            !string.IsNullOrWhiteSpace(SmtpUsername) &&
            !string.IsNullOrWhiteSpace(SmtpPassword);
    }
}
