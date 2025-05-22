using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Components;

namespace NetGuardUI.Pages.Identity
{
    public class LogoutModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogoutModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string ReturnUrl { get; private set; }
        public async Task<IActionResult> OnGetAsync()
        {
            try { if (_httpContextAccessor.HttpContext != null) { await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); } }
            catch (Exception ex) { string error = ex.Message; }
            return LocalRedirect("/");
        }
    }
}
