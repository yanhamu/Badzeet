using System;
using System.Linq;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Badzeet.Web.Api;

[Route("api/accounts")]
public class AccountsController : ControllerBase
{
    private readonly IUserAccountRepository userAccountRepository;

    public AccountsController(
        IUserAccountRepository userAccountRepository)
    {
        this.userAccountRepository = userAccountRepository;
    }

    [HttpGet]
    public async Task<object> List()
    {
        var userId = Guid.Parse(User.FindFirst("sub")!.Value);
        var userAccounts = await userAccountRepository.GetUserAccounts(userId);

        return userAccounts.Select(x => new AccountDto
        {
            Id = x.Account.Id,
            FirstDayOfTheBudget = x.Account.FirstDayOfTheBudget,
            CreatedAt = x.Account.CreatedAt
        });
    }

    public class AccountDto
    {
        public long Id { get; set; }
        public int FirstDayOfTheBudget { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}