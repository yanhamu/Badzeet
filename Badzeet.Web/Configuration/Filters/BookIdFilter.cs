using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Configuration.Filters
{
    public class BookIdFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionDescriptor.Parameters.Any(x => x.Name == "bookId" && x.ParameterType == typeof(long)))
            {
                var bookId = context.HttpContext.GetBookId();
                context.ActionArguments.Add("bookId", bookId);
            }

            await next();
        }
    }
}