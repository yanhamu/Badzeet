using Badzeet.Budget.Domain;
using System;

namespace Badzeet.Web.Features.Budget
{
    public class BudgetViewModel
    {
        public int BudgetId { get; set; }
        public BudgetCategoryViewModel[] Categories { get; set; }
        public decimal Spend { get; set; }
        public decimal Budget { get; set; }
        public decimal RemainingBudget { get { return Budget - Spend; } }
        public DateInterval BudgetInterval { get; set; }
        public bool IsOngoing { get { return DateTime.Now.Date <= BudgetInterval.To && DateTime.Now.Date >= BudgetInterval.From; } }

    }
}
