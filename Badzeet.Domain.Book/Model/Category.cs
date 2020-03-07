namespace Badzeet.Domain.Book.Model
{
    public class Category
    {
        public long Id { get; set; }
        public long BookId { get; set; }
        public Book Book { get; set; }
        public string Name { get; set; }
    }
}