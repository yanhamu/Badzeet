namespace Badzeet.Budget.Domain.Model
{
    public class Category
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public Account Account { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }
}