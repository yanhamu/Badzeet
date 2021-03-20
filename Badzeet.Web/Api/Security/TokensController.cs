using Badzeet.User.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Badzeet.Web.Api.Security
{
    public class TokensController : ControllerBase
    {
        private readonly ITokenService tokenService;

        public TokensController(ITokenService tokenService)
        {
            this.tokenService = tokenService;
        }

        [Route("api/login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Request request)
        {
            var result = await tokenService.Issue(request.Username, request.Password);
            if (result.IsSuccess == false)
                return new StatusCodeResult(403);

            return Ok(result);
        }
    }

    public class Request
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
