using MediSphere.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace MediSphere.Pages.Administrator
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

            var userEmail = existingUser.Email;
            var result = await _userManager.DeleteAsync(existingUser);

            if (result.Succeeded && !string.IsNullOrEmpty(userEmail))
            {
                await _emailSender.SendEmailAsync(
                    userEmail,
                    "MediSphere account removed",
                    "Your MediSphere account has been removed from the system by an administrator.");
            }

            if (!result.Succeeded)
            {
                return Page();
            }

            return RedirectToPage("/Administrator/Settings");
        }
    }
}
