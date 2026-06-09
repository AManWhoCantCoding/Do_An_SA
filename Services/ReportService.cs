using MediSphere.Models;
using MediSphere.Persistence.Interfaces;
using MediSphere.Services.Interfaces;

namespace MediSphere.Services
{
    public class ReportService : IReportService
    {
        private readonly IRepository<ReportModel> _repository;

        public ReportService(IRepository<ReportModel> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<ReportModel>> GetAllAsync() => _repository.GetAsync();

        public Task<ReportModel> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<ReportModel> CreateAsync(ReportModel report) => _repository.CreateAsync(report);

        public Task<ReportModel> UpdateAsync(ReportModel report) => _repository.UpdateAsync(report);

        public async Task MarkAsPrintedAsync(int reportId)
        {
            var report = await _repository.GetByIdAsync(reportId);
            report.IsReportPrinted = true;
            await _repository.UpdateAsync(report);
        }
    }
}
