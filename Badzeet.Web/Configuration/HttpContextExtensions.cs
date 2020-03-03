using Microsoft.AspNetCore.Http;

namespace Badzeet.Web.Configuration
{
    public static class HttpContextExtensions
    {
        public static long GetAccountId(this HttpContext context)
        {
            return long.Parse(context.Items["da"].ToString());
        }
    }
}
