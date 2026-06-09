using MediSphere.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MediSphere.DAL
{
    public class ApplicationDBContext : IdentityDbContext<UserModel, IdentityRole<int>, int>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public DbSet<AppointmentModel> Appointments { get; set; }
        public DbSet<PatientModel> Patients { get; set; }
        public DbSet<PrescriptionModel> Prescriptions { get; set; }
        public DbSet<ReportModel> Reports { get; set; }
        public DbSet<ReportTypeModel> ReportTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ReportTypeModel>().HasData(
        new ReportTypeModel { ReportTypeId = 1, TemplateType = "Medical Examination Report" },
        new ReportTypeModel { ReportTypeId = 2, TemplateType = "Hospital Revenue Report" },
        new ReportTypeModel { ReportTypeId = 3, TemplateType = "Doctor Shift & Duty Report" },
        new ReportTypeModel { ReportTypeId = 4, TemplateType = "Medication Dispensation Report" },
        new ReportTypeModel { ReportTypeId = 5, TemplateType = "Patient Admission Statistics Report" },
        new ReportTypeModel { ReportTypeId = 6, TemplateType = "Clinical Laboratory Test Report" },
        new ReportTypeModel { ReportTypeId = 7, TemplateType = "Medical Supplies & Consumables Report" },
        new ReportTypeModel { ReportTypeId = 8, TemplateType = "Clinical Research & Trials Report" });// Thêm báo cáo nghiên cứu ở đây
        }
    }
}
