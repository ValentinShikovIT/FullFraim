using FullFraim.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Shared;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Utilities.Mailing;

namespace Web.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(UserManager<User> userManager,
            IConfiguration config,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _config = config;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                TempData["Success"] = Constants.TempDataNotifications.PasswordResetTokenSuccessfullySend;

                var user = await _userManager.FindByEmailAsync(Input.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return RedirectToPage("./Login");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code },
                    protocol: Request.Scheme);

                await this._emailSender.SendEmailAsync(
                        Sender: this._config["SendGrid:SenderEmail"],
                        SenderName: Constants.Email.SenderName,
                        To: user.Email,
                        Subject: Constants.Email.ResetPasswordSubject,
                        HtmlContent: string.Format(Constants.EmailContents.PasswordResetLink,
                            HtmlEncoder.Default.Encode(callbackUrl)));

                return RedirectToPage("./Login");
            }

            return Page();
        }
    }
}
