using MediSphere.DAL;
using MediSphere.Models;
using MediSphere.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediSphere.Persistence
{
    public class ReportRepository : IRepository<ReportModel>
    {
        private readonly ApplicationDBContext _dbcontext;

        public ReportRepository(ApplicationDBContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<ReportModel> CreateAsync(ReportModel entity)
        {
            await _dbcontext.Reports.AddAsync(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<ReportModel>> GetAsync()
        {
            return await _dbcontext.Reports
                .Include(r => r.ReportTypeModel)
                .ToListAsync();
        }

        public async Task<ReportModel> GetByIdAsync(int id)
        {
            var report = await _dbcontext.Reports
                .Include(r => r.ReportTypeModel)
                .FirstOrDefaultAsync(r => r.ReportId == id);
            if (report == null)
            {
                throw new KeyNotFoundException($"Report {id} not found.");
            }
            return report;
        }

        public async Task<ReportModel> UpdateAsync(ReportModel entity)
        {
            _dbcontext.Entry(entity).CurrentValues.SetValues(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;
        }

        public async Task<ReportModel> DeleteAsync(ReportModel entity)
        {
            _dbcontext.Reports.Remove(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;
        }
    }
}
