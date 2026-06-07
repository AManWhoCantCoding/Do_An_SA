using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DocDocGo.Pages.Prescriptions
{
    [Authorize]
    public class AddPrescriptionModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
