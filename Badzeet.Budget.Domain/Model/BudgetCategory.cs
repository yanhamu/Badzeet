﻿using System;

namespace Badzeet.Budget.Domain.Model
{
    public class BudgetCategory
    {
        public int BudgetId { get; set; }
        public long AccountId { get; set; }
        public Budget Budget { get; set; } = default!;
        public Guid CategoryId { get; set; }
        public Category Category { get; set; } = default!;
        public decimal Amount { get; set; }
    }
}