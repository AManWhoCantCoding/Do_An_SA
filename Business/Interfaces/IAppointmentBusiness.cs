using MediSphere.Business;
using MediSphere.Dto;

namespace MediSphere.Business.Interfaces
{
    public interface IAppointmentBusiness
    {
        Task<IEnumerable<AppointmentDto>> GetAllAsync();
        Task<AppointmentDto?> GetByIdAsync(int id);
        Task<BusinessResult<AppointmentDto>> CreateAsync(CreateAppointmentDto dto);
        Task<BusinessResult<AppointmentDto>> UpdateAsync(int id, CreateAppointmentDto dto);
        Task<BusinessResult<bool>> DeleteAsync(int id);
    }
}
