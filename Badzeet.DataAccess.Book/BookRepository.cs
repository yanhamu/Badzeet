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

        public async Task<Domain.Book.Model.Account> CreateBook(byte firstDayOfBudget)
        {
            var book = new Domain.Book.Model.Account() { FirstDayOfTheBudget = firstDayOfBudget };
            var bookEntity = dbContext
                .Set<Domain.Book.Model.Account>()
                .Add(book);
            await dbContext.SaveChangesAsync();
            return bookEntity.Entity;
        }

        public async Task<Domain.Book.Model.Account> GetBook(long bookId)
        {
            return await dbContext
                .Set<Domain.Book.Model.Account>()
                .FindAsync(bookId);
        }
    }
}
