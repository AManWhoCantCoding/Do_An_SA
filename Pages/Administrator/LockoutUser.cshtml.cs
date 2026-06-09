using MediSphere.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace MediSphere.Pages.Administrator
{
    [Authorize(Roles = "Administrator")]
    public class LockoutUserModel : PageModel
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IEmailSender _emailSender;

        public LockoutUserModel(UserManager<UserModel> userManager, IEmailSender emailSender)
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

            await _userManager.SetLockoutEndDateAsync(existingUser, DateTimeOffset.MaxValue);

            if (!string.IsNullOrEmpty(existingUser.Email))
            {
                await _emailSender.SendEmailAsync(
                    existingUser.Email,
                    "MediSphere account locked",
                    "Your MediSphere account has been locked by an administrator. Contact your hospital administrator for assistance.");
            }

            return RedirectToPage("/Administrator/Settings");
        }

    }
}
