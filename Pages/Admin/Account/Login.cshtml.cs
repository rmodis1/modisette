using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Modisette.Services;

namespace Modisette.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAdminAuthenticationService _adminAuthenticationService;

        public LoginModel(IAdminAuthenticationService adminAuthenticationService)
        {
            _adminAuthenticationService = adminAuthenticationService;
        }

        [BindProperty]
        [Required]
        [StringLength(100)]
        public string Username { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; } = "/Admin/ContactForm/Display";

        public IActionResult OnGet() => Page();

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var authenticationResult = _adminAuthenticationService.Authenticate(Username, Password);
            if (!authenticationResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, authenticationResult.ErrorMessage ?? "Login failed.");
                return Page();
            }

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, Username.Trim()),
                new(ClaimTypes.Role, "Admin")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true });

            if (!Url.IsLocalUrl(ReturnUrl))
            {
                ReturnUrl = "/Admin/ContactForm/Display";
            }

            return LocalRedirect(ReturnUrl);
        }
    }
}
