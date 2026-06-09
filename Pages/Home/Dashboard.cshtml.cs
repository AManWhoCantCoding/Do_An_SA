using MediSphere.Business.Interfaces;
using MediSphere.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MediSphere.Pages.Home
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        private readonly IDashboardBusiness _dashboardBusiness;

        public DashboardStats Stats { get; private set; } = new();

        public DashboardModel(IDashboardBusiness dashboardBusiness)
        {
            _dashboardBusiness = dashboardBusiness;
        }

        public async Task OnGetAsync()
        {
            Stats = await _dashboardBusiness.GetDashboardStatsAsync();
        }
    }
}
