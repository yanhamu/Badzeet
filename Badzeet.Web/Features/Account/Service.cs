using Badzeet.Service.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Account
{
    public class Service
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IUserService userService;

        public Service(IHttpContextAccessor contextAccessor, IUserService userService)
        {
            this.contextAccessor = contextAccessor;
            this.userService = userService;
        }

        public async Task<bool> Login(UserCredentialsModel credentials)
        {
            var userResponse = await userService.Check(credentials.Username, credentials.Password);
            if (userResponse.IsSuccessful == false)
                return false;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, credentials.Username),
                new Claim("Id", userResponse.GetUserId().ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties();

            await contextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return true;
        }

        public async Task<bool> Register(UserCredentialsModel credentials)
        {
            if (await userService.CheckAvailability(credentials.Username) == false)
                return false;

            await userService.RegisterUser(credentials.Username, credentials.Password);
            return true;
        }

        public async Task Logout()
        {
            await contextAccessor.HttpContext.SignOutAsync();
        }
    }
}