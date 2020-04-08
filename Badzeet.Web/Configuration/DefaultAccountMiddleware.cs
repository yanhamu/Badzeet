using Badzeet.Domain.Book.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Configuration
{
    public class DefaultAccountMiddleware
    {
        private readonly RequestDelegate next;

        public DefaultAccountMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
        public async Task InvokeAsync(HttpContext context, IUserAccountService service)
        {
            if (context.User.Identity.IsAuthenticated == true)
            {
                var claim = context.User.Claims.FirstOrDefault(c => c.Type == "Id");

                var userId = Guid.Parse(claim.Value);

                var bookId = await service.GetBookId(userId);
                context.Items["da"] = bookId;

                if (context.Request.Cookies.ContainsKey("da") == false)
                    context.Response.Cookies.Append("da", bookId.ToString());
            }

            await next(context);
        }
    }

    public interface IUserAccountService
    {
        public Task<long> GetBookId(Guid userId);
    }

    public class UserAccountService : IUserAccountService
    {
        private readonly IUserBookRepository userBookRepository;

        public UserAccountService(IUserBookRepository userBookRepository)
        {
            this.userBookRepository = userBookRepository;
        }

        public async Task<long> GetBookId(Guid userId)
        {
            return (await userBookRepository.GetBooks(userId)).First().AccountId;
        }
    }
}