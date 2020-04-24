namespace Badzeet.Budget.Domain.Model
{
    public class CategoryBudget
    {
        public int Id { get; set; }
        public long AccountId { get; set; }
        public Account Account { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public decimal Amount { get; set; }
    }
}
