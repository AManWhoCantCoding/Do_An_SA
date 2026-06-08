using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MediSphere.Pages.Reports
{
    [Authorize]
    public class ReportsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
