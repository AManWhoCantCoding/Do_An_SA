using MediSphere.Services.Interfaces;

namespace MediSphere.Business.Interfaces
{
    public interface IDashboardBusiness
    {
        Task<DashboardStats> GetDashboardStatsAsync();
    }
}
