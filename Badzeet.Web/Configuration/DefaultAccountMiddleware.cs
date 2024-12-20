﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Badzeet.Web.Configuration;

public class DefaultAccountMiddleware
{
    private readonly RequestDelegate next;

    public DefaultAccountMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context, IUserAccountService service)
    {
        if (context.Request.Path.StartsWithSegments(new PathString("/api")) == false)
            if (context.User.Identity!.IsAuthenticated)
            {
                var userId = context.GetUserId();

                var accountId = await service.GetAccountId(userId);
                context.Items["da"] = accountId;
                context.Items["ui"] = userId;

                if (context.Request.Cookies.ContainsKey("da") == false)
                    context.Response.Cookies.Append("da", accountId.ToString());
            }

        await next(context);
    }
}

public interface IUserAccountService
{
    public Task<long> GetAccountId(Guid userId);
}

public class UserAccountService : IUserAccountService
{
    private readonly IUserAccountRepository userBookRepository;

    public UserAccountService(IUserAccountRepository userBookRepository)
    {
        this.userBookRepository = userBookRepository;
    }

    public async Task<long> GetAccountId(Guid userId)
    {
        return (await userBookRepository.GetUserAccounts(userId)).First().AccountId;
    }
}