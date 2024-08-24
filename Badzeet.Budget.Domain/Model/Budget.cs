using System;

namespace Badzeet.Budget.Domain.Model;

public class Budget
{
    public Guid Id { get; set; }
    public int BudgetId { get; set; }
    public long AccountId { get; set; }
    public Account Account { get; set; } = default!;
    public DateTime Date { get; set; }
    public DateInterval Interval => new(Date, Date.AddMonths(1).AddTicks(-1));
}