using MediSphere.Models;

namespace MediSphere.Services.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<ReportModel>> GetAllAsync();
        Task<ReportModel> GetByIdAsync(int id);
        Task<ReportModel> CreateAsync(ReportModel report);
        Task<ReportModel> UpdateAsync(ReportModel report);
        Task MarkAsPrintedAsync(int reportId);
    }
}
