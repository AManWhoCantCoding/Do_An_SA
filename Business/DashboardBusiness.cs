using MediSphere.Business.Interfaces;
using MediSphere.Services.Interfaces;

namespace MediSphere.Business
{
    public class DashboardBusiness : IDashboardBusiness
    {
        private readonly IDashboardService _dashboardService;

        public DashboardBusiness(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        public Task<DashboardStats> GetDashboardStatsAsync() => _dashboardService.GetStatsAsync();
    }
}
