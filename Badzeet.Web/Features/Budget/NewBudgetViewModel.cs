using System;
using System.Collections.Generic;

namespace Badzeet.Web.Features.Budget;

public class NewBudgetViewModel
{
    public NewBudgetViewModel()
    {
    }

    public NewBudgetViewModel(List<ComparisonCategoryBudgetViewModel> budgets, int @new, DateTime from, DateTime to)
    {
        Budgets = budgets;
        New = @new;
        From = from;
        To = to;
    }

    public List<ComparisonCategoryBudgetViewModel> Budgets { get; set; } = new();
    public int New { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}

public class ComparisonCategoryBudgetViewModel
{
    public long CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal OldAmount { get; set; }
}