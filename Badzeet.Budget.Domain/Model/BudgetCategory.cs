namespace Badzeet.Budget.Domain.Model
{
    public class BudgetCategory
    {
        public long BudgetId { get; set; }
        public Budget Budget { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public decimal Amount { get; set; }
    }
}