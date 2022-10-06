using System;

namespace Badzeet.Web.Features.Budget
{
    public class CategoryBudgetViewModel
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
    }
}
