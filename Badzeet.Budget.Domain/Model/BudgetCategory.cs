namespace Badzeet.Budget.Domain.Model
{
    public class BudgetCategory
    {
        public int BudgetId { get; set; }
        public long AccountId { get; set; }
        public Budget Budget { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public decimal Amount { get; set; }

        // TODO deal with category_budget table
    }
}