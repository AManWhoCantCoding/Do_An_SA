using DocDocGo.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DocDocGo.Services;
using Microsoft.AspNetCore.Authorization;

namespace DocDocGo.Pages.Administrator
{
    [Authorize(Roles = "Administrator")]
    public class DeleteUserModel : PageModel
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IEmailSender _emailSender;

        public DeleteUserModel(UserManager<UserModel> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public UserModel ExistingUserModel { get; set; }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            ExistingUserModel = await _userManager.FindByIdAsync(id.ToString());

            if (ExistingUserModel == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var existingUser = await _userManager.FindByIdAsync(id.ToString());

            if (existingUser == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(existingUser);
            //you could implement the sending of an email to the user, to tell them that their account has been removed from the system here.

            if (!result.Succeeded)
            {
                return Page();
            }

            return RedirectToPage("/Administrator/Settings");
        }
    }
}
