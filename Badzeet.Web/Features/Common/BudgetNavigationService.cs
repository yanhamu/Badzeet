using System;
using System.Globalization;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;

namespace Badzeet.Web.Features.Common;

public class BudgetNavigationService
{
    private readonly IAccountRepository accountRepository;
    private readonly IBudgetRepository budgetRepository;

    public BudgetNavigationService(
        IBudgetRepository budgetRepository,
        IAccountRepository accountRepository)
    {
        this.budgetRepository = budgetRepository;
        this.accountRepository = accountRepository;
    }

    public async Task<BudgetNavigationViewModel> Get(long accountId, int budgetId)
    {
        var account = await accountRepository.GetAccount(accountId);
        var budget = await budgetRepository.Get(budgetId, accountId);
        var budgetIdString = budgetId + account.FirstDayOfTheBudget.ToString("D2");
        var date = DateTime.ParseExact(budgetIdString, "yyyyMMdd", CultureInfo.InvariantCulture);
        var previousDate = date.AddMonths(-1);
        var currentDate = date;
        var nextDate = date.AddMonths(1);

        var previousId = int.Parse(previousDate.ToString("yyyyMM"));
        var currentId = budgetId;
        var nextId = int.Parse(nextDate.ToString("yyyyMM"));

        return new BudgetNavigationViewModel
        {
            Current = new BudgetNavigationItemViewModel
            {
                BudgetId = currentId,
                FirstBudgetDate = currentDate
            },
            Next = new BudgetNavigationItemViewModel
            {
                BudgetId = nextId,
                FirstBudgetDate = nextDate
            },
            Previous = new BudgetNavigationItemViewModel
            {
                BudgetId = previousId,
                FirstBudgetDate = previousDate
            },
            HasBudget = budget != null
        };
    }

    public async Task<BudgetNavigationViewModel> Get(long accountId)
    {
        var account = await accountRepository.GetAccount(accountId);
        var now = DateTime.UtcNow;

        var date = new DateTime(now.Year, now.Month, account.FirstDayOfTheBudget);
        var budgetId = int.Parse(date.ToString("yyyyMM"));
        if (now >= date)
            return await Get(accountId, budgetId);

        var previousBudgetId = int.Parse(date.AddMonths(-1).ToString("yyyyMM"));
        return await Get(accountId, previousBudgetId);
    }
}