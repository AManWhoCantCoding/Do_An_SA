using DocDocGo.Models;
using DocDocGo.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Build.Framework;

namespace DocDocGo.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<UserModel> _signInManager;
        private readonly ILogger<LoginModel> _logger;

        public LoginModel(SignInManager<UserModel> signInManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;

        }

        [BindProperty, Required]
        public LoginViewModel CredentialModel { get; set; }

        public void OnGet()
        {
        
        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(CredentialModel.Email, CredentialModel.Password, CredentialModel.RememberMe, lockoutOnFailure: true);

                    if (result.Succeeded)
                    {
                    _logger.LogInformation("User successfully logged in");

                    return RedirectToPage("/Home/Dashboard"); // Redirect to the main dashboard.
                    }
                
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { RememberMe = CredentialModel.RememberMe });
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }

                ModelState.AddModelError(string.Empty, "Invalid credentials, Please try again!");
            }

            return Page();
        }

    }
}
