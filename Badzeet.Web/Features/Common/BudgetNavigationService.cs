using Badzeet.Budget.Domain.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Common
{
    public class BudgetNavigationService
    {
        private readonly IBudgetRepository budgetRepository;
        private readonly IAccountRepository accountRepository;

        public BudgetNavigationService(
            IBudgetRepository budgetRepository,
            IAccountRepository accountRepository)
        {
            this.budgetRepository = budgetRepository;
            this.accountRepository = accountRepository;
        }

        public async Task<BudgetNavigationViewModel> Get(long accountId, DateTime date)
        {
            var previous = date.AddMonths(-1);
            var current = date;
            var next = date.AddMonths(1);

            var budgets = await budgetRepository.List(accountId, new Filter() { From = previous, To = next });

            return new BudgetNavigationViewModel()
            {
                Current = new BudgetNavigationItemViewModel()
                {
                    BudgetId = budgets.SingleOrDefault(x => x.Date == current)?.BudgetId,
                    FirstBudgetDate = current
                },
                Next = new BudgetNavigationItemViewModel()
                {
                    BudgetId = budgets.SingleOrDefault(x => x.Date == next)?.BudgetId,
                    FirstBudgetDate = next
                },
                Previous = new BudgetNavigationItemViewModel()
                {
                    BudgetId = budgets.SingleOrDefault(x => x.Date == previous)?.BudgetId,
                    FirstBudgetDate = previous
                }
            };
        }

        public async Task<BudgetNavigationViewModel> Get(long accountId)
        {
            var account = await accountRepository.GetAccount(accountId);
            var now = DateTime.UtcNow;

            var date = new DateTime(now.Year, now.Month, account.FirstDayOfTheBudget);
            if (now >= date)
                return await Get(accountId, date);

            return await Get(accountId, date.AddMonths(-1));
        }
    }
}
