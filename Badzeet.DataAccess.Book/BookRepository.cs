using Badzeet.Domain.Book.Interfaces;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Book
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext dbContext;

        public BookRepository(BookDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Domain.Book.Model.Book> CreateBook(byte firstDayOfBudget)
        {
            var book = new Domain.Book.Model.Book() { FirstDayOfTheBudget = firstDayOfBudget };
            var bookEntity = dbContext
                .Set<Domain.Book.Model.Book>()
                .Add(book);
            await dbContext.SaveChangesAsync();
            return bookEntity.Entity;
        }

        public async Task<Domain.Book.Model.Book> GetBook(long bookId)
        {
            return await dbContext
                .Set<Domain.Book.Model.Book>()
                .FindAsync(bookId);
        }
    }
}
