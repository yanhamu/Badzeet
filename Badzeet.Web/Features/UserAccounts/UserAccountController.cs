﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Badzeet.Web.Features.UserAccounts;

public class UserAccountController : Controller
{
    private readonly IUserAccountRepository userAccountRepository;

    public UserAccountController(IUserAccountRepository userAccountRepository)
    {
        this.userAccountRepository = userAccountRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid userId, long accountId)
    {
        var accounts = await userAccountRepository.GetUserAccounts(userId);
        var a = accounts.Where(x => x.AccountId == accountId)
            .Select(x => new UserAccountViewModel
            {
                Name = x.User.Nickname,
                UserId = x.UserId
            })
            .Single();
        return View(a);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid userId, long accountId, object temporary)
    {
        var accounts = await userAccountRepository.GetUserAccounts(userId);
        var a = accounts.Where(x => x.AccountId == accountId).Single();

        await userAccountRepository.Save();

        return RedirectToAction("Index", "Budget");
    }

    public class UserAccountViewModel
    {
        public string Name { get; set; } = default!;
        public Guid UserId { get; set; }
        public decimal? Balance { get; set; }
    }
}