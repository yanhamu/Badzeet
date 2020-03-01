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
        private readonly IUserService repository;

        public Service(IHttpContextAccessor contextAccessor, IUserService userRepository)
        {
            this.contextAccessor = contextAccessor;
            this.repository = userRepository;
        }

        public async Task<bool> Login(UserCredentialsModel credentials)
        {
            var userResponse = await repository.Check(credentials.Username, credentials.Password);
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
            if (await repository.CheckAvailability(credentials.Username) == false)
                return false;

            await repository.RegisterUser(credentials.Username, credentials.Password);
            return true;
        }

        public async Task Logout()
        {
            await contextAccessor.HttpContext.SignOutAsync();
        }
    }
}