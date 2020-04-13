using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Configuration.Filters
{
    public class AccountIdFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionDescriptor.Parameters.Any(x => x.Name == "bookId" && x.ParameterType == typeof(long)))
            {
                var accountId = context.HttpContext.GetBookId();
                context.ActionArguments.Add("bookId", accountId);
            }

            if (context.ActionDescriptor.Parameters.Any(x => x.Name == "accountId" && x.ParameterType == typeof(long)))
            {
                var accountId = context.HttpContext.GetBookId();
                context.ActionArguments.Add("accountId", accountId);
            }

            await next();
        }
    }
}