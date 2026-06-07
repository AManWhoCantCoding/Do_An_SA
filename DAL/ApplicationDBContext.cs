using DocDocGo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DocDocGo.DAL
{ 
/// <summary>
/// Represents the application's database context, extending identitydbcontext.
/// This context manages all interactions with the database, including authentication and authorization.
/// </summary>
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
