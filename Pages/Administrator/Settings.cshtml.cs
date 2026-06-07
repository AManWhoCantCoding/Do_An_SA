using DocDocGo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DocDocGo.Pages.Administrator
{
    [Authorize(Roles = "Administrator")]
    public class SettingsModel : PageModel
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IEmailSender _emailSender;
        public IEnumerable<UserModel> Users { get; set; }

        public SettingsModel(UserManager<UserModel> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        public async Task<IActionResult> OnGet()
        {
            Users = await _userManager.Users.ToListAsync();
            return Page();
        }


        public async Task<IActionResult> OnGetUsersWithRoleAsync(string roleName)
        {
            Users = await _userManager.GetUsersInRoleAsync(roleName);

            return Page();
        }

        public void OnPost() 
        { 
        }

        public async Task<IActionResult> LockUserAccountAsync(string userId) //locks a specific user fully out of their account.
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                await _userManager.SetLockoutEnabledAsync(user, true);
                await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
            }

            return RedirectToPage();
        }

    }
}
