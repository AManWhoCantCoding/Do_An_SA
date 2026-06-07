using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DocDocGo.Pages.Patient
{
    [Authorize]
    public class AddPatientModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
