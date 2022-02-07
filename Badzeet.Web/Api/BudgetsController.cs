using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Api
{
    [Route("api/accounts/{accountId:long}/budgets")]
    public class BudgetsController : ControllerBase
    {
        private readonly IBudgetRepository budgetRepository;
        private readonly IAccountRepository accountRepository;

        public BudgetsController(
            IBudgetRepository budgetRepository,
            IAccountRepository accountRepository)
        {
            this.budgetRepository = budgetRepository;
            this.accountRepository = accountRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get(long accountId)
        {
            var account = await accountRepository.GetAccount(accountId);
            var budgets = await budgetRepository.List(accountId, new Filter());

            var dtos = budgets.Select(x => new BudgetDto()
            {
                AccountId = accountId,
                BudgetId = x.BudgetId,
                From = GetStartDate(x.BudgetId, account.FirstDayOfTheBudget),
                To = GetLastDate(x.BudgetId, account.FirstDayOfTheBudget),
            });

            return Ok(dtos);
        }

        [HttpGet("{budgetId:int}")]
        public async Task<IActionResult> Get(long accountId, int budgetId)
        {
            var account = await accountRepository.GetAccount(accountId);
            var budget = await budgetRepository.Get(budgetId, accountId);
            if (budget == null)
                return NotFound();

            var startDate = GetStartDate(budgetId, account.FirstDayOfTheBudget);
            var endDate = GetLastDate(budgetId, account.FirstDayOfTheBudget);

            return Ok(new BudgetDto()
            {
                AccountId = account.Id,
                BudgetId = budgetId,
                From = startDate,
                To = endDate
            });
        }

        private DateTime GetStartDate(int budgetId, int firstDay)
        {
            return DateTime.ParseExact(budgetId.ToString() + firstDay.ToString("D2"), "yyyyMMdd", CultureInfo.InvariantCulture);
        }

        private DateTime GetLastDate(int budgetId, int firstDay)
        {
            var startDate = GetStartDate(budgetId, firstDay);
            return startDate.AddMonths(1).AddTicks(-1);
        }

        public class BudgetDto
        {
            public long AccountId { get; set; }
            public int BudgetId { get; set; }
            public DateTime From { get; set; }
            public DateTime To { get; set; }
        }
    }
}
