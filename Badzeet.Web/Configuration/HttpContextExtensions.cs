using Microsoft.AspNetCore.Http;

namespace Badzeet.Web.Configuration
{
    public static class HttpContextExtensions
    {
        public static long GetBookId(this HttpContext context)
        {
            return long.Parse(context.Items["da"].ToString());
        }
    }
}
