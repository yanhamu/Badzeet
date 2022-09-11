using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Configuration.Filters
{
    public class AccountIdFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionDescriptor.Parameters.Any(x => x.Name == "accountId" && x.ParameterType == typeof(Guid))
                && (context.ActionArguments.ContainsKey("accountId") == false))
            {
                context.ActionArguments.Add("accountId", context.HttpContext.GetAccountId());
            }

            await next();
        }
    }
}