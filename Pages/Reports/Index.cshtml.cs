using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DocDocGo.Pages.Reports
{
    [Authorize]
    public class ReportsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
