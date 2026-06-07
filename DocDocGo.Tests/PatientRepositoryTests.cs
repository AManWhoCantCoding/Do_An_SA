using DocDocGo.DAL;
using DocDocGo.Models;
using DocDocGo.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DocDocGo.Tests
{
    public class PatientRepositoryTests
    {
        private static ApplicationDBContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new ApplicationDBContext(options);
        }

        [Fact]
        public async Task CreateAsync_AddsPatient()
        {
            await using var context = CreateContext();
            var repository = new PatientRepository(context);
            var patient = new PatientModel
            {
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john@example.com"
            };

            var result = await repository.CreateAsync(patient);

            Assert.True(result.PatientId > 0);
            Assert.Single(await repository.GetAsync());
        }

        [Fact]
        public async Task UpdateAsync_ModifiesPatient()
        {
            await using var context = CreateContext();
            var repository = new PatientRepository(context);
            var patient = await repository.CreateAsync(new PatientModel
            {
                FirstName = "Jane",
                LastName = "Smith"
            });

            patient.LastName = "Updated";
            await repository.UpdateAsync(patient);
            var updated = await repository.GetByIdAsync(patient.PatientId);

            Assert.Equal("Updated", updated.LastName);
        }

        [Fact]
        public async Task DeleteAsync_RemovesPatient()
        {
            await using var context = CreateContext();
            var repository = new PatientRepository(context);
            var patient = await repository.CreateAsync(new PatientModel
            {
                FirstName = "Delete",
                LastName = "Me"
            });

            await repository.DeleteAsync(patient);

            Assert.Empty(await repository.GetAsync());
        }
    }
}
