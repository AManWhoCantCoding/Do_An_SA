using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MediSphere.Pages.Prescriptions
{
    [Authorize]
    public class UpdatePrescriptionModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int PrescriptionId { get; set; }

        public IActionResult OnGet(int id)
        {
            PrescriptionId = id;
            if (id <= 0)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
