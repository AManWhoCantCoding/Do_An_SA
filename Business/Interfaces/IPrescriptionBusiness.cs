using MediSphere.Business;
using MediSphere.Dto;

namespace MediSphere.Business.Interfaces
{
    public interface IPrescriptionBusiness
    {
        Task<IEnumerable<PrescriptionDto>> GetAllAsync();
        Task<PrescriptionDto?> GetByIdAsync(int id);
        Task<IEnumerable<PrescriptionDto>> GetByPatientIdAsync(int patientId);
        Task<BusinessResult<PrescriptionDto>> CreateAsync(CreatePrescriptionDto dto);
        Task<BusinessResult<PrescriptionDto>> UpdateAsync(int id, CreatePrescriptionDto dto);
        Task<BusinessResult<bool>> DeleteAsync(int id);
    }
}
