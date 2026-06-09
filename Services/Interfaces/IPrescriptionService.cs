using MediSphere.Models;

namespace MediSphere.Services.Interfaces
{
    public interface IPrescriptionService
    {
        Task<IEnumerable<PrescriptionModel>> GetAllAsync();
        Task<PrescriptionModel> GetByIdAsync(int id);
        Task<IEnumerable<PrescriptionModel>> GetByPatientIdAsync(int patientId);
        Task<PrescriptionModel> CreateAsync(PrescriptionModel prescription);
        Task<PrescriptionModel> UpdateAsync(PrescriptionModel prescription);
        Task DeleteAsync(int id);
    }
}
