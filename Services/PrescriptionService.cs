using MediSphere.Models;
using MediSphere.Persistence.Interfaces;
using MediSphere.Services.Interfaces;

namespace MediSphere.Services
{
    public class PrescriptionService : IPrescriptionService
    {
        private readonly IRepository<PrescriptionModel> _repository;

        public PrescriptionService(IRepository<PrescriptionModel> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<PrescriptionModel>> GetAllAsync() => _repository.GetAsync();

        public Task<PrescriptionModel> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public async Task<IEnumerable<PrescriptionModel>> GetByPatientIdAsync(int patientId)
        {
            var prescriptions = await _repository.GetAsync();
            return prescriptions.Where(p => p.PatientId == patientId);
        }

        public Task<PrescriptionModel> CreateAsync(PrescriptionModel prescription) => _repository.CreateAsync(prescription);

        public Task<PrescriptionModel> UpdateAsync(PrescriptionModel prescription) => _repository.UpdateAsync(prescription);

        public async Task DeleteAsync(int id)
        {
            var prescription = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(prescription);
        }
    }
}
