using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Account
{
    public class AccountController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register(string invitationId)
        {
            ViewData["invitationId"] = invitationId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserCredentialsModel credentials, Guid? invitationId)
        {
            var registrationResult = await Register(credentials);
            return LocalRedirect("/");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(UserCredentialsModel credentials, string returnUrl)
        {
            var loginResult = await Login(credentials);

            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            await HttpContext.SignOutAsync();
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return View();
        }

        private Task<bool> Register(UserCredentialsModel credentials)
        {
            return Task.FromResult(true);
        }

        private async Task<bool> Login(UserCredentialsModel credentials)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, credentials.Username),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties();

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return true;
        }

    }
}