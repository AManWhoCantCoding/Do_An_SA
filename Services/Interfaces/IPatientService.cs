using MediSphere.Models;

namespace MediSphere.Services.Interfaces
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientModel>> GetAllAsync();
        Task<PatientModel> GetByIdAsync(int id);
        Task<PatientModel> CreateAsync(PatientModel patient);
        Task<PatientModel> UpdateAsync(PatientModel patient);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
