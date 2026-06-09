using MediSphere.Models;

namespace MediSphere.Services.Interfaces
{
    public interface IAppointmentService
    {
        Task<IEnumerable<AppointmentModel>> GetAllAsync();
        Task<AppointmentModel> GetByIdAsync(int id);
        Task<AppointmentModel> CreateAsync(AppointmentModel appointment);
        Task<AppointmentModel> UpdateAsync(AppointmentModel appointment);
        Task DeleteAsync(int id);
    }
}
