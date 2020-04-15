using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Configuration.Filters
{
    public class UserIdFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionDescriptor.Parameters.Any(x => x.Name == "userId" && x.ParameterType == typeof(Guid)))
                context.ActionArguments.Add("userId", context.HttpContext.GetUserId());
            await next();
        }
    }
}