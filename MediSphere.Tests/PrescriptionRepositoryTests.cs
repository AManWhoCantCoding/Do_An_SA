using MediSphere.DAL;
using MediSphere.Models;
using MediSphere.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MediSphere.Tests
{
    public class PrescriptionRepositoryTests
    {
        private static ApplicationDBContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDBContext(options);
        }

        [Fact]
        public async Task CreateAsync_AddsPrescription()
        {
            await using var context = CreateContext();
            var repository = new PrescriptionRepository(context);

            var prescription = await repository.CreateAsync(new PrescriptionModel
            {
                PatientId = 1,
                MedicationName = "Paracetamol",
                Dosage = "500mg"
            });

            Assert.True(prescription.PrescriptionId > 0);
        }
    }
}
