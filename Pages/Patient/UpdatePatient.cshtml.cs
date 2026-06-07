using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DocDocGo.Pages.Patient
{
    [Authorize]
    public class UpdatePatientModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int PatientId { get; set; }

        public IActionResult OnGet(int id)
        {
            PatientId = id;
            if (id <= 0)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
