using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Badzeet.Web.Api
{
    [Route("api/accounts/{accountId:long}")]
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

        [HttpGet("budgets/{budgetId:int}")]
        public async Task<IActionResult> Get(long accountId, int budgetId)
        {
            var account = await accountRepository.GetAccount(accountId);
            var budget = await budgetRepository.Get(budgetId, accountId);
            if (budget == null)
                return NotFound();

            var startDate = DateTime.ParseExact(budget.BudgetId.ToString() + account.FirstDayOfTheBudget.ToString("D2"), "yyyyMMdd", CultureInfo.InvariantCulture);
            var endDate = startDate.AddMonths(1).AddTicks(-1);

            return Ok(new BudgetDto()
            {
                AccountId = account.Id,
                BudgetId = budgetId,
                From = startDate,
                To = endDate
            });
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
