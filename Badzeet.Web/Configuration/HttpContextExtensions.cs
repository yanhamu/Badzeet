using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Badzeet.Web.Configuration;

public static class HttpContextExtensions
{
    public static long GetAccountId(this HttpContext context)
    {
        return long.Parse(context.Items["da"].ToString());
    }

    public static Guid GetUserId(this HttpContext context)
    {
        var claim = context.User.Claims.FirstOrDefault(c => c.Type == "sub");
        return Guid.Parse(claim.Value);
    }
}