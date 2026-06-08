using NuGet.Packaging.Core;
using MediSphere.DAL;
using MediSphere.Models;
using MediSphere.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MediSphere.Repositories
{
    public class AppointmentRepository : IAppointmentRepository<AppointmentModel>
    {
        private readonly ApplicationDBContext _dbcontext;

        public AppointmentRepository(ApplicationDBContext dbContext)
        {
            _dbcontext = dbContext;
        }

        public async Task<AppointmentModel> CreateAsync(AppointmentModel entity)
        {
            await _dbcontext.Appointments.AddAsync(entity);

            await _dbcontext.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<AppointmentModel>> GetAsync()
        {
            return await _dbcontext.Appointments.ToListAsync();
        }

        public async Task<AppointmentModel> GetByIdAsync(int id)
        {
            var appointment = await _dbcontext.Appointments.FindAsync(id);
            if (appointment == null)
            {

                throw new Exception("appointment not found");
            }
            return appointment;
        }

        public async Task<AppointmentModel> UpdateAsync(AppointmentModel entity)
        {
            _dbcontext.Entry(entity).CurrentValues.SetValues(entity);

            await _dbcontext.SaveChangesAsync();

            return entity;
        }

        public async Task<AppointmentModel> DeleteAsync(AppointmentModel entity)
        {
            _dbcontext.Remove(entity);
            await _dbcontext.SaveChangesAsync();
            return entity;
        }
    }
    
}
