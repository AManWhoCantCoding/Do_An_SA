namespace MediSphere.Services.Interfaces
{
    public class DashboardStats
    {
        public int PatientCount { get; set; }
        public int AppointmentCount { get; set; }
        public int PrescriptionCount { get; set; }
        public int ReportCount { get; set; }
        public int UpcomingAppointmentCount { get; set; }
    }

    public interface IDashboardService
    {
        Task<DashboardStats> GetStatsAsync();
    }
}
