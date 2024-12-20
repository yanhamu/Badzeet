﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Badzeet.Web.Features.Account;

public class AccountController : Controller
{
    private readonly Service service;

    public AccountController(Service service)
    {
        this.service = service;
    }

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
    public async Task<IActionResult> Register(UserCredentialsModel credentials)
    {
        if (!ModelState.IsValid)
            return View();

        await service.Register(credentials);
        return LocalRedirect("/");
    }

    [HttpGet]
    public IActionResult Login(string returnUrl)
    {
        ViewData["returnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(UserCredentialsModel credentials, string returnUrl)
    {
        var loginResult = await service.Login(credentials, returnUrl);

        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return Redirect("/");
    }

    [HttpPost]
    public async Task<IActionResult> Logout(string returnUrl)
    {
        await service.Logout();
        if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);

        return Redirect("/");
    }
}