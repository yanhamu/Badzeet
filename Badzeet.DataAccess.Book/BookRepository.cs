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
        public async Task<Domain.Book.Model.Book> GetBook(long bookId)
        {
            return await dbContext
                .Set<Domain.Book.Model.Book>()
                .FindAsync(bookId);
        }
    }
}
