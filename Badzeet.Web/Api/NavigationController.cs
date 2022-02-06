using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Api
{
    [Route("api")]
    public class NavigationController : ControllerBase
    {
        private readonly IBudgetRepository budgetRepository;
        private readonly IAccountRepository accountRepository;

        public NavigationController(IBudgetRepository budgetRepository, IAccountRepository accountRepository)
        {
            this.budgetRepository = budgetRepository;
            this.accountRepository = accountRepository;
        }

        [HttpGet("accounts/{accountId:long}/navigations")]
        public async Task<NavigationResponse> Get(long accountId)
        {
            var now = DateTime.UtcNow.Date;
            var budget = (await budgetRepository.List(accountId, new Filter() { From = now, To = now })).SingleOrDefault();
            var account = await accountRepository.GetAccount(accountId);
            var firstDay = (int)account.FirstDayOfTheBudget;

            var budgetDateBoundary = now.AddDays(-now.Day + 1).AddDays(firstDay - 1);
            if (budgetDateBoundary >= now)
                return new NavigationResponse(budgetDateBoundary.AddMonths(-1), budgetDateBoundary.AddDays(-1), budget?.BudgetId);

            return new NavigationResponse(budgetDateBoundary, budgetDateBoundary.AddMonths(1).AddDays(-1), budget?.BudgetId);
        }

        public class NavigationResponse
        {
            public NavigationResponse(DateTime from, DateTime to, int? budgetId = null)
            {
                BudgetId = budgetId;
                From = from;
                To = to;
            }

            public int? BudgetId { get; set; }
            public DateTime From { get; set; }
            public DateTime To { get; set; }
        }
    }
}
