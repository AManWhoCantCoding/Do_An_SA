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
    public class UpdateUserModel : PageModel
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        public List<IdentityRole<int>> AllRoles { get; set; }

        public UpdateUserModel(UserManager<UserModel> userManager, IEmailSender emailSender, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
        }

        [BindProperty]
        public UserModel ExistingUserModel { get; set; } = default!;
        [BindProperty]
        public string SelectedRole { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ExistingUserModel = await _userManager.FindByIdAsync(id.ToString());

            if (ExistingUserModel == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(ExistingUserModel);

            AllRoles = await _roleManager.Roles.ToListAsync();
            
            SelectedRole = userRoles.FirstOrDefault();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var existingUser = await _userManager.FindByIdAsync(ExistingUserModel.Id.ToString());

            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.FirstName = ExistingUserModel.FirstName;
            existingUser.LastName = ExistingUserModel.LastName;
            existingUser.Email = ExistingUserModel.Email;
            existingUser.PhoneNumber = ExistingUserModel.PhoneNumber;
            existingUser.Gender = ExistingUserModel.Gender;
            existingUser.DateOfBirth = ExistingUserModel.DateOfBirth;
            existingUser.TwoFactorEnabled = existingUser.TwoFactorEnabled;

            await _userManager.UpdateAsync(existingUser);
            
            if (!string.IsNullOrEmpty(SelectedRole))
            {
                var role = await _roleManager.FindByNameAsync(SelectedRole);
               
                if (role != null)
                {                    
                    await _userManager.AddToRoleAsync(existingUser, SelectedRole);
                }

            }
            return RedirectToPage("/Administrator/Settings");

        }

    }
}
