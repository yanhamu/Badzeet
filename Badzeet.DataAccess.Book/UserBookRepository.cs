using Badzeet.Domain.Book.Interfaces;
using Badzeet.Domain.Book.Model;
using Microsoft.EntityFrameworkCore;
using System;
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

        public async Task<UserBook> Create(Guid userId, string username, long bookId)
        {
            var userBook = new UserBook()
            {
                BookId = bookId,
                UserId = userId,
                Nickname = username
            };
            var trackedEntity = context.Set<UserBook>()
                .Add(userBook);
            await context.SaveChangesAsync();
            return trackedEntity.Entity;
        }

        public async Task<IEnumerable<UserBook>> GetBooks(Guid userId)
        {
            return await context.Set<UserBook>()
                .Where(x => x.UserId == userId)
                .ToListAsync();
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
