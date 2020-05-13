using Badzeet.Budget.Domain;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Configuration.Filters
{
    public class BudgetIdFilter : IAsyncActionFilter
    {
        private const string parameterName = "budgetId";
        private const string cookieName = "mbid";
        private readonly BudgetService BudgetService;

        public BudgetIdFilter(BudgetService bookService)
        {
            this.BudgetService = bookService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (HasArgument(context) && ProvidedValue(context) == false)
            {
                var accountId = context.HttpContext.GetAccountId();
                var budgetId = await GetBudgetId(context, accountId);
                context.ActionArguments.Add(parameterName, budgetId);
            }

            if (HasArgument(context) && ProvidedValue(context))
            {
                var budgetId = (int)context.ActionArguments[parameterName];
                context.HttpContext.Response.Cookies.Append(cookieName, budgetId.ToString());
            }

            await next();
        }

        private async Task<int> GetBudgetId(ActionExecutingContext context, long accountId)
        {
            if (context.HttpContext.Request.Cookies.ContainsKey(cookieName))
                return short.Parse(context.HttpContext.Request.Cookies[cookieName]);

            return await BudgetService.GetLatestBudgetId(accountId);
        }

        private static bool ProvidedValue(ActionExecutingContext context)
        {
            return context.ActionArguments.ContainsKey(parameterName);
        }

        private static bool HasArgument(ActionExecutingContext context)
        {
            return context.ActionDescriptor.Parameters.Any(x => x.Name == parameterName && x.ParameterType == typeof(int));
        }
    }
}
