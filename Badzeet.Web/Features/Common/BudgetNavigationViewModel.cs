using System;

namespace Badzeet.Web.Features.Common;

public class BudgetNavigationViewModel
{
    public BudgetNavigationItemViewModel Previous { get; set; } = default!;
    public BudgetNavigationItemViewModel Current { get; set; } = default!;
    public BudgetNavigationItemViewModel Next { get; set; } = default!;
    public bool HasBudget { get; set; }
}

public class BudgetNavigationItemViewModel
{
    public DateTime FirstBudgetDate { get; set; }
    public int BudgetId { get; set; }
}