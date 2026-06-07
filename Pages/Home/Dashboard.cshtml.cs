using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DocDocGo.Pages.Home
{
    [Authorize]
    public class DashboardModel : PageModel
    {
        public void OnGet()
        {
        }
        public void OnPost() 
        { 
        }
    }
}
