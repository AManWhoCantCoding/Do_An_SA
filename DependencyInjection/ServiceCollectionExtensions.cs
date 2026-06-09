using MediSphere.Business;
using MediSphere.Business.Interfaces;
using MediSphere.DAL;
using MediSphere.Models;
using MediSphere.Persistence;
using MediSphere.Persistence.Interfaces;
using MediSphere.Services;
using MediSphere.Services.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MediSphere.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMediSphereLayers(this IServiceCollection services)
        {
            // Persistence Layer
            services.AddScoped<IAppointmentRepository<AppointmentModel>, AppointmentRepository>();
            services.AddScoped<IRepository<PatientModel>, PatientRepository>();
            services.AddScoped<IRepository<PrescriptionModel>, PrescriptionRepository>();
            services.AddScoped<IRepository<ReportModel>, ReportRepository>();
            services.AddScoped<IRepository<ReportTypeModel>, ReportTypeRepository>();

            // Services Layer
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IPrescriptionService, PrescriptionService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IReportTypeService, ReportTypeService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddTransient<IEmailSender, EmailSender>();

            // Business Layer
            services.AddScoped<IPatientBusiness, PatientBusiness>();
            services.AddScoped<IAppointmentBusiness, AppointmentBusiness>();
            services.AddScoped<IPrescriptionBusiness, PrescriptionBusiness>();
            services.AddScoped<IReportBusiness, ReportBusiness>();
            services.AddScoped<IDashboardBusiness, DashboardBusiness>();

            return services;
        }
    }
}
