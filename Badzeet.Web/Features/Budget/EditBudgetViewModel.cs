﻿using System.Collections.Generic;

namespace Badzeet.Web.Features.Budget;

public class EditBudgetViewModel
{
    public List<CategoryBudgetViewModel> Budgets { get; set; }
    public long BudgetId { get; internal set; }
}