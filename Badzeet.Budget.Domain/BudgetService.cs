using System;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;

namespace Badzeet.Budget.Domain;

public class BudgetService
{
    private readonly IBudgetRepository budgetRepository;

    public BudgetService(IBudgetRepository budgetRepository)
    {
        this.budgetRepository = budgetRepository;
    }

    public async Task<DateInterval> GetMonthlyBudgetById(int budgetId, long accountId) //TODO remove
    {
        var budget = (await budgetRepository.Get(budgetId, accountId))!;
        return budget.Interval;
    }

    public DateInterval GetBudgetInterval(byte firstDay, DateTime date)
    {
        DateTime startDate;
        if (date.Day >= firstDay)
            startDate = new DateTime(date.Year, date.Month, firstDay);
        else
            startDate = new DateTime(date.AddMonths(-1).Year, date.AddMonths(-1).Month, firstDay);

        var endDate = startDate.AddMonths(1).AddDays(-1);

        return new DateInterval(startDate, endDate);
    }
}