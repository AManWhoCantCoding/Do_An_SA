using DocDocGo.DAL;
using DocDocGo.Models;
using DocDocGo.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DocDocGo.Repositories
{
    public class ReportTypeRepository : IRepository<ReportTypeModel>
    {
        private readonly ApplicationDBContext _dbcontext;

        public ReportTypeRepository(ApplicationDBContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<ReportTypeModel> CreateAsync(ReportTypeModel entity)
        {
            await _dbcontext.ReportTypes.AddAsync(entity);

            await _dbcontext.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<ReportTypeModel>> GetAsync()
        {
            return await _dbcontext.ReportTypes.ToListAsync();
        }

        public async Task<ReportTypeModel> GetByIdAsync(int id)
        {
            var report = await _dbcontext.ReportTypes.FindAsync(id);
            if (report == null)
            {

                throw new Exception("report type not found");
            }
            return report;
        }

        public async Task<ReportTypeModel> UpdateAsync(ReportTypeModel entity)
        {
            _dbcontext.Entry(entity).CurrentValues.SetValues(entity);

            await _dbcontext.SaveChangesAsync();

            return entity;
        }

        public async Task<ReportTypeModel> DeleteAsync(ReportTypeModel entity)
        {
            _dbcontext.ReportTypes.Remove(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;
        }
    }
}
