using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Badzeet.Web.Configuration
{
    public static class HttpContextExtensions
    {
        public static Guid GetAccountId(this HttpContext context)
        {
            return Guid.Parse(context.Items["da"].ToString());
        }

        public static Guid GetUserId(this HttpContext context)
        {
            var claim = context.User.Claims.FirstOrDefault(c => c.Type == "sub");
            return Guid.Parse(claim.Value);
        }
    }
}