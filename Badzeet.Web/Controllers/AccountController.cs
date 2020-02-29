using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Badzeet.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(string username, string password, string returnUrl)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogoutAsync(string returnUrl)
        {
            await HttpContext.SignOutAsync();
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            return View();
        }
    }
}