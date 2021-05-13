using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Badzeet.Web.Api
{
    [Route("api")]
    public class TestController : ControllerBase
    {
        [HttpGet("test")]
        public string Test()
        {
            return "my test";
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("auth")]
        public object Auth()
        {
            return new { name = "random name" };
        }
    }
}
