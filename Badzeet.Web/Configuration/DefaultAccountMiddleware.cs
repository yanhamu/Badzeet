using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Badzeet.Web.Configuration
{
    public class DefaultAccountMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IUserAccountService service;

        public DefaultAccountMiddleware(RequestDelegate next, IUserAccountService service)
        {
            this.next = next;
            this.service = service;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated == true)
            {
                var accountId = service.GetAccountId(context.User.Identity.Name);
                context.Items["da"] = accountId;

                if (context.Request.Cookies.ContainsKey("da") == false)
                    context.Response.Cookies.Append("da", accountId.ToString());
            }

            await next(context);
        }
    }

    public interface IUserAccountService
    {
        long GetAccountId(string name);
    }

    public class UserAccountService : IUserAccountService
    {
        public long GetAccountId(string name)
        {
            return 1;
        }
    }
}