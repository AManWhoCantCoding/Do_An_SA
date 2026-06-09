using MediSphere.Models;

namespace MediSphere.Services.Interfaces
{
    public interface IReportTypeService
    {
        Task<IEnumerable<ReportTypeModel>> GetAllAsync();
        Task<ReportTypeModel> GetByIdAsync(int id);
        Task<ReportTypeModel> CreateAsync(ReportTypeModel reportType);
    }
}
