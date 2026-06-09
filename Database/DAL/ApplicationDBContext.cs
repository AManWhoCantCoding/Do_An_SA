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
        }
    }
}
