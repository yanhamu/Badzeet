using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.UserAccounts
{
    public class UserAccountController : Controller
    {
        private readonly IUserAccountRepository userAccountRepository;

        public UserAccountController(IUserAccountRepository userAccountRepository)
        {
            this.userAccountRepository = userAccountRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid userId, long accountId)
        {
            var accounts = await userAccountRepository.GetUserAccounts(userId);
            var a = accounts.Where(x => x.AccountId == accountId)
                .Select(x => new UserAccountViewModel()
                {
                    Balance = x.Balance,
                    Name = x.User.Nickname,
                    UserId = x.UserId
                })
                .Single();
            return View(a);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid userId, long accountId, decimal balance)
        {
            var accounts = await userAccountRepository.GetUserAccounts(userId);
            var a = accounts.Where(x => x.AccountId == accountId).Single();

            a.Balance = balance;

            await userAccountRepository.Save();

            return RedirectToAction("Index", "Budget");
        }

        public class UserAccountViewModel
        {
            public string Name { get; set; }
            public Guid UserId { get; set; }
            public decimal? Balance { get; set; }
        }
    }
}
