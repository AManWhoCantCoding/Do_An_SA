using MediSphere.Models;
using MediSphere.Persistence.Interfaces;
using MediSphere.Services.Interfaces;

namespace MediSphere.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository<AppointmentModel> _repository;

        public AppointmentService(IAppointmentRepository<AppointmentModel> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<AppointmentModel>> GetAllAsync() => _repository.GetAsync();

        public Task<AppointmentModel> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<AppointmentModel> CreateAsync(AppointmentModel appointment) => _repository.CreateAsync(appointment);

        public Task<AppointmentModel> UpdateAsync(AppointmentModel appointment) => _repository.UpdateAsync(appointment);

        public async Task DeleteAsync(int id)
        {
            var appointment = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(appointment);
        }
    }
}
