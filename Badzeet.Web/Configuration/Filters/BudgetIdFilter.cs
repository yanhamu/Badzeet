using Badzeet.Domain.Book;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Configuration.Filters
{
    public class BudgetIdFilter : IAsyncActionFilter
    {
        private const string parameterName = "budgetId";
        private const string cookieName = "mbid";
        private readonly BookService bookService;

        public BudgetIdFilter(BookService bookService)
        {
            this.bookService = bookService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (HasArgument(context) && ProvidedValue(context) == false)
            {
                var bookId = context.HttpContext.GetAccountId();
                var budgetId = await GetBudgetId(context, bookId);
                context.ActionArguments.Add(parameterName, budgetId);
            }

            if (HasArgument(context) && ProvidedValue(context))
            {
                var budgetId = (int)context.ActionArguments[parameterName];
                context.HttpContext.Response.Cookies.Append(cookieName, budgetId.ToString());
            }

            await next();
        }

        private async Task<int> GetBudgetId(ActionExecutingContext context, long bookId)
        {
            if (context.HttpContext.Request.Cookies.ContainsKey(cookieName))
                return short.Parse(context.HttpContext.Request.Cookies[cookieName]);

            return await bookService.GetLatestBudgetId(bookId);
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
