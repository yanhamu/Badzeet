using Badzeet.Domain.Book;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Configuration.Filters
{
    public class BudgetIdFilter : IAsyncActionFilter
    {
        private const string parameterName = "budgetId";
        private readonly BookService bookService;

        public BudgetIdFilter(BookService bookService)
        {
            this.bookService = bookService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (HasArgument(context) && ProvidedValue(context) == false)
            {
                var bookId = context.HttpContext.GetBookId();
                var budgetId = await GetBudgetId(context, bookId);
                context.ActionArguments.Add(parameterName, budgetId);
            }

            if (HasArgument(context) && ProvidedValue(context))
            {
                var budgetId = (int)context.ActionArguments[parameterName];
                context.HttpContext.Response.Cookies.Append(parameterName, budgetId.ToString());
            }

            await next();
        }

        private async Task<int> GetBudgetId(ActionExecutingContext context, long bookId)
        {
            if (context.HttpContext.Request.Cookies.ContainsKey(parameterName))
                return int.Parse(context.HttpContext.Request.Cookies[parameterName]);

            return await bookService.GetLatestBudgetOffset(bookId);
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
