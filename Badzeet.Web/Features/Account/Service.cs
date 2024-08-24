using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Badzeet.Budget.Domain;
using Badzeet.User.Domain;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace Badzeet.Web.Features.Account;

public class Service
{
    private readonly IHttpContextAccessor contextAccessor;
    private readonly RegistrationService registrationService;
    private readonly IUserService userService;

    public Service(
        IHttpContextAccessor contextAccessor,
        IUserService userService,
        RegistrationService registrationService)
    {
        this.contextAccessor = contextAccessor;
        this.userService = userService;
        this.registrationService = registrationService;
    }

    public async Task<bool> Login(UserCredentialsModel credentials, string returnUrl)
    {
        var userResponse = await userService.Check(credentials.Username, credentials.Password);
        if (userResponse.IsSuccessful == false) return false;

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, credentials.Username),
            new("sub", userResponse.UserId.ToString())
        };

        var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

        await contextAccessor.HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            claimsPrincipal);

        return true;
    }

    public async Task<bool> Register(UserCredentialsModel credentials)
    {
        if (await userService.CheckAvailability(credentials.Username) == false)
            return false;

        var userId = await userService.RegisterUser(credentials.Username, credentials.Password);

        await registrationService.Register(userId, credentials.Username);
        return true;
    }

    public async Task Logout()
    {
        contextAccessor.HttpContext.Response.Cookies.Delete("budgetId");
        contextAccessor.HttpContext.Response.Cookies.Delete("da");

        await contextAccessor.HttpContext.SignOutAsync();
    }
}