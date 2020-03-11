using Badzeet.Domain.Book.Interfaces;
using Badzeet.Domain.Book.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Book
{
    public class UserBookRepository : IUserBookRepository
    {
        private readonly BookDbContext context;

        public UserBookRepository(BookDbContext context)
        {
            this.context = context;
        }
        public async Task<IEnumerable<UserBook>> GetUsers(long bookId)
        {
            return await context
                .Set<UserBook>()
                .Where(x => x.BookId == bookId)
                .ToListAsync();
        }
    }
}
