using MediSphere.DAL;
using MediSphere.Models;
using MediSphere.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediSphere.Persistence
{
    public class PatientRepository : IRepository<PatientModel>
    {
        private readonly ApplicationDBContext _dbcontext;

        public PatientRepository(ApplicationDBContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<PatientModel> CreateAsync(PatientModel entity)
        {
            await _dbcontext.Patients.AddAsync(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<PatientModel>> GetAsync()
        {
            return await _dbcontext.Patients.ToListAsync();
        }

        public async Task<PatientModel> GetByIdAsync(int id)
        {
            var patient = await _dbcontext.Patients.FindAsync(id);
            if (patient == null)
            {
                throw new KeyNotFoundException($"Patient {id} not found.");
            }
            return patient;
        }

        public async Task<PatientModel> UpdateAsync(PatientModel entity)
        {
            _dbcontext.Entry(entity).CurrentValues.SetValues(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;
        }

        public async Task<PatientModel> DeleteAsync(PatientModel entity)
        {
            _dbcontext.Patients.Remove(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;
        }
    }
}
