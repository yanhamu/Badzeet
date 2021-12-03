using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Api.Filters
{
    public class BudgetIdFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userId = new Guid(context.HttpContext.User.Claims.Single(x => x.Type == "sub").Value);
            var budgetId = (long)context.ActionArguments["budgetId"];

            var userAccountRepository = (IUserAccountRepository)context.HttpContext.RequestServices.GetService(typeof(IUserAccountRepository));
            var budgetRepository = (IBudgetRepository)context.HttpContext.RequestServices.GetService(typeof(IBudgetRepository));

            var budget = await budgetRepository.Get(budgetId);

            if (budget == null)
            {
                context.Result = new StatusCodeResult(404);
            }
            else
            {
                var accounts = await userAccountRepository.GetUserAccounts(userId);
                if (accounts.Any(a => a.AccountId == budget.AccountId))
                {
                    await next();

                }
                else
                {
                    context.Result = new StatusCodeResult(403);
                }
            }
        }
    }
}
