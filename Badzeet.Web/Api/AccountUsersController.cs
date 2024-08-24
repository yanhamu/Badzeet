using System;
using System.Linq;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Badzeet.Web.Api;

[Route("api/accounts/{accountId:long}/users")]
public class AccountUsersController : ControllerBase
{
    private readonly IUserAccountRepository userAccountRepository;

    public AccountUsersController(IUserAccountRepository userAccountRepository)
    {
        this.userAccountRepository = userAccountRepository;
    }

    [HttpGet]
    public async Task<IActionResult> List(long accountId)
    {
        var userAccounts = await userAccountRepository.GetUsers(accountId);
        return Ok(userAccounts.Select(x => new UserAccountDto(x.UserId, x.User.Nickname)));
    }

    public class UserAccountDto
    {
        public UserAccountDto(Guid id, string nick)
        {
            Id = id;
            Nick = nick;
        }

        public Guid Id { get; set; }
        public string Nick { get; set; }
    }
}