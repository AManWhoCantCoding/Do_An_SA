using MediSphere.Business;
using MediSphere.Dto;

namespace MediSphere.Business.Interfaces
{
    public interface IPatientBusiness
    {
        Task<IEnumerable<PatientDto>> GetAllAsync();
        Task<PatientDto?> GetByIdAsync(int id);
        Task<BusinessResult<PatientDto>> CreateAsync(CreatePatientDto dto);
        Task<BusinessResult<PatientDto>> UpdateAsync(int id, CreatePatientDto dto);
        Task<BusinessResult<bool>> DeleteAsync(int id);
    }
}
