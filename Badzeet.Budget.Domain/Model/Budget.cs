using System;

namespace Badzeet.Budget.Domain.Model
{
    public class Budget
    {
        public int BudgetId { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
        public DateTime Date { get; set; }
        public DateInterval Interval { get => new DateInterval(Date, Date.AddMonths(1).AddTicks(-1)); }
    }
}
