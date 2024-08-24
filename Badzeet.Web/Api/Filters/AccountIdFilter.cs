using System;
using System.Linq;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Badzeet.Web.Api.Filters;

public class AccountIdFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var accountId = (long)context.ActionArguments["accountId"]!;
        var repository = context.HttpContext.RequestServices.GetRequiredService<IUserAccountRepository>();

        var userId = new Guid(context.HttpContext.User.Claims.Single(x => x.Type == "sub").Value);
        var accounts = await repository.GetUserAccounts(userId);
        if (accounts.Any(a => a.AccountId == accountId))
            await next();
        else
            context.Result = new StatusCodeResult(403);
    }
}