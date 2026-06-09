using MediSphere.Models;
using MediSphere.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace MediSphere.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<UserModel> userManager,
            SignInManager<UserModel> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public UserModel CredentialModel { get; set; } = new();

        [BindProperty]
        public PasswordViewModel PWordModel { get; set; } = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (!CredentialModel.AcceptedTerms)
            {
                ModelState.AddModelError(nameof(CredentialModel.AcceptedTerms), "Please accept the terms and conditions.");
                return Page();
            }

            var user = new UserModel
            {
                Email = CredentialModel.Email,
                DateOfBirth = CredentialModel.DateOfBirth,
                PhoneNumber = CredentialModel.PhoneNumber,
                FirstName = CredentialModel.FirstName,
                MiddleName = CredentialModel.MiddleName,
                LastName = CredentialModel.LastName,
                Gender = CredentialModel.Gender,
                UserName = CredentialModel.Email,
                CreatedAt = DateTime.Now,
                AcceptedTerms = CredentialModel.AcceptedTerms
            };

            var result = await _userManager.CreateAsync(user, PWordModel.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return Page();
            }

            await _userManager.AddToRoleAsync(user, "Staff");

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { userId, code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
                CredentialModel.Email,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            if (_userManager.Options.SignIn.RequireConfirmedAccount)
            {
                return RedirectToPage("/Account/RegisterConfirmation", new { email = CredentialModel.Email });
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToPage("/Home/Dashboard");
        }
    }
}
