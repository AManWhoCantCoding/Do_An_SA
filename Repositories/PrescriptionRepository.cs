using MediSphere.DAL;
using MediSphere.Models;
using MediSphere.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediSphere.Repositories
{
    public class PrescriptionRepository : IRepository<PrescriptionModel>
    {
        private ApplicationDBContext _dbcontext;
        
        public PrescriptionRepository(ApplicationDBContext dbContext)
        {
                _dbcontext = dbContext;
        }

        public async Task<PrescriptionModel> CreateAsync(PrescriptionModel entity)
        {
            await _dbcontext.Prescriptions.AddAsync(entity);
           
            await _dbcontext.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<PrescriptionModel>> GetAsync()
        {
            return await _dbcontext.Prescriptions.ToListAsync();
        }

        public async Task<PrescriptionModel> GetByIdAsync(int id)
        {
            var prescription = await _dbcontext.Prescriptions.FindAsync(id);
            if (prescription == null)
            {

                throw new Exception("prescription not found");
            }
            return prescription;
        }

        public async Task<PrescriptionModel> UpdateAsync(PrescriptionModel entity)
        {
            _dbcontext.Entry(entity).CurrentValues.SetValues(entity);

            await _dbcontext.SaveChangesAsync();

            return entity;
        }

        public async Task<PrescriptionModel> DeleteAsync(PrescriptionModel entity)
        {
            _dbcontext.Prescriptions.Remove(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;
        }
    }
}
