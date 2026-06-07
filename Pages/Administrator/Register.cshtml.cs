using DocDocGo.Models;
using DocDocGo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;

namespace DocDocGo.Pages.Administrator
{
    [Authorize(Roles = "Administrator")]
    public class RegisterModel : PageModel
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly SignInManager<UserModel> _signInManager;
        private readonly IEmailSender _emailSender;

        public RegisterModel(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        public UserModel CredentialModel { get; set; }

        [BindProperty]
        public PasswordViewModel PWordModel { get; set; }


        public async Task OnGetAsync()
        {
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel
                {
                    Email = CredentialModel.Email,
                    DateOfBirth = CredentialModel.DateOfBirth,
                    PhoneNumber = CredentialModel.PhoneNumber,
                    FirstName = CredentialModel.FirstName,
                    MiddleName = CredentialModel?.MiddleName,
                    LastName = CredentialModel.LastName,
                    Gender = CredentialModel.Gender,
                    UserName = CredentialModel.Email,
                    CreatedAt = DateTime.Now,
                    AcceptedTerms = CredentialModel.AcceptedTerms
                };

                if (!user.AcceptedTerms)
                {
                    ModelState.AddModelError("ValidationError", "Please accept the terms and conditions");
                    return Page();
                }
                var result = await _userManager.CreateAsync(user, PWordModel.Password);

                if (result.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId, code },
                        protocol: Request.Scheme);

                    //Not needed below for purpose of assignment, but if you wanted to configure your own smtp port settings you could, it works as intended, so you can send any email.
                    //await _emailSender.SendEmailAsync(CredentialModel.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = CredentialModel.Email });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return RedirectToPage("/Home/Dashboard"); // Redirect to main dashboard
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return Page();
        }
    }
}
