using Badzeet.Budget.Domain;
using Badzeet.User.Domain;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Account
{
    public class Service
    {
        private readonly IHttpContextAccessor contextAccessor;
        private readonly IUserService userService;
        private readonly RegistrationService registrationService;
        private readonly IEventService events;
        private readonly IIdentityServerInteractionService interaction;

        public Service(
            IHttpContextAccessor contextAccessor,
            IUserService userService,
            RegistrationService registrationService,
            IEventService events,
            IIdentityServerInteractionService interaction)
        {
            this.contextAccessor = contextAccessor;
            this.userService = userService;
            this.registrationService = registrationService;
            this.events = events;
            this.interaction = interaction;
        }

        public async Task<bool> Login(UserCredentialsModel credentials, string returnUrl)
        {
            var context = await interaction.GetAuthorizationContextAsync(returnUrl);
            var userResponse = await userService.Check(credentials.Username, credentials.Password);
            if (userResponse.IsSuccessful == false)
            {
                await events.RaiseAsync(new UserLoginFailureEvent(credentials.Username, "invalid credentials", clientId: context?.Client.ClientId));
                return false;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, credentials.Username),
                new Claim("sub", userResponse.UserId.ToString()),
                new Claim("rand","rand")
            };

            await events.RaiseAsync(new UserLoginSuccessEvent(credentials.Username, userResponse.UserId.ToString(), credentials.Username, clientId: context?.Client.ClientId));

            var isuser = new IdentityServerUser(userResponse.UserId.ToString())
            {
                DisplayName = credentials.Username,
                AdditionalClaims = claims
            };
            
            await contextAccessor.HttpContext.SignInAsync(isuser);

            //await contextAccessor.HttpContext.SignInAsync(
            //    CookieAuthenticationDefaults.AuthenticationScheme,
            //    new ClaimsPrincipal(claimsIdentity),
            //    authProperties);

            return true;
        }

        public async Task<bool> Register(UserCredentialsModel credentials, Guid? invitationId)
        {
            if (await userService.CheckAvailability(credentials.Username) == false)
                return false;

            var userId = await userService.RegisterUser(credentials.Username, credentials.Password);

            await registrationService.Register(userId, credentials.Username, invitationId);
            return true;
        }

        public async Task Logout()
        {
            contextAccessor.HttpContext.Response.Cookies.Delete("budgetId");
            contextAccessor.HttpContext.Response.Cookies.Delete("da");

            await contextAccessor.HttpContext.SignOutAsync();
        }
    }
}