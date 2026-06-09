using MediSphere.Services.Interfaces;

namespace MediSphere.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IPatientService _patientService;
        private readonly IAppointmentService _appointmentService;
        private readonly IPrescriptionService _prescriptionService;
        private readonly IReportService _reportService;

        public DashboardService(
            IPatientService patientService,
            IAppointmentService appointmentService,
            IPrescriptionService prescriptionService,
            IReportService reportService)
        {
            _patientService = patientService;
            _appointmentService = appointmentService;
            _prescriptionService = prescriptionService;
            _reportService = reportService;
        }

        public async Task<DashboardStats> GetStatsAsync()
        {
            var patients = await _patientService.GetAllAsync();
            var appointments = await _appointmentService.GetAllAsync();
            var prescriptions = await _prescriptionService.GetAllAsync();
            var reports = await _reportService.GetAllAsync();
            var now = DateTime.UtcNow;

            return new DashboardStats
            {
                PatientCount = patients.Count(),
                AppointmentCount = appointments.Count(),
                PrescriptionCount = prescriptions.Count(),
                ReportCount = reports.Count(),
                UpcomingAppointmentCount = appointments.Count(a => a.StartTime > now)
            };
        }
    }
}
