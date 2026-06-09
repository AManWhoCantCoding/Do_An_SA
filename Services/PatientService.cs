using MediSphere.Models;
using MediSphere.Persistence.Interfaces;
using MediSphere.Services.Interfaces;

namespace MediSphere.Services
{
    public class PatientService : IPatientService
    {
        private readonly IRepository<PatientModel> _repository;

        public PatientService(IRepository<PatientModel> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<PatientModel>> GetAllAsync() => _repository.GetAsync();

        public Task<PatientModel> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<PatientModel> CreateAsync(PatientModel patient) => _repository.CreateAsync(patient);

        public Task<PatientModel> UpdateAsync(PatientModel patient) => _repository.UpdateAsync(patient);

        public async Task DeleteAsync(int id)
        {
            var patient = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(patient);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                await _repository.GetByIdAsync(id);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
    }
}
