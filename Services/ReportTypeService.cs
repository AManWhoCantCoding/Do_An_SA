using MediSphere.Models;
using MediSphere.Persistence.Interfaces;
using MediSphere.Services.Interfaces;

namespace MediSphere.Services
{
    public class ReportTypeService : IReportTypeService
    {
        private readonly IRepository<ReportTypeModel> _repository;

        public ReportTypeService(IRepository<ReportTypeModel> repository)
        {
            _repository = repository;
        }

        public Task<IEnumerable<ReportTypeModel>> GetAllAsync() => _repository.GetAsync();

        public Task<ReportTypeModel> GetByIdAsync(int id) => _repository.GetByIdAsync(id);

        public Task<ReportTypeModel> CreateAsync(ReportTypeModel reportType) => _repository.CreateAsync(reportType);
    }
}
