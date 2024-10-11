using System.Collections.Generic;

namespace Badzeet.Web.Features.Budget;

public class NewBudgetViewModel(List<ComparisonCategoryBudgetViewModel> budgets, long @new)
{
    public List<ComparisonCategoryBudgetViewModel> Budgets { get; set; } = budgets;
    public long New { get; internal set; } = @new;
}

public class ComparisonCategoryBudgetViewModel
{
    public long CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal OldAmount { get; set; }
}