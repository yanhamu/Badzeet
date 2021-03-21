using Badzeet.User.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
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

        [Route("api/tokens")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Request request)
        {
            var result = await tokenService.Issue(request.Username, request.Password);
            if (result.IsSuccess == false)
                return new StatusCodeResult(403);

            return Ok(result);
        }

        [Route("api/tokens")]
        [HttpPut]
        public async Task<IActionResult> Login([FromBody] RefreshRequest request)
        {
            var result = await tokenService.Refresh(request.UserId, request.Token);
            return Ok(result);
        }
    }

    public class RefreshRequest
    {
        public Guid UserId { get; set; }
        public string Token { get; set; }
    }

    public class Request
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
