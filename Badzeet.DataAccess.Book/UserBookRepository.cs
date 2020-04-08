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

        public async Task<UserAccount> Create(Guid userId, long bookId)
        {
            var userBook = new UserAccount()
            {
                AccountId = bookId,
                UserId = userId
            };
            var trackedEntity = context.Set<UserAccount>()
                .Add(userBook);
            await context.SaveChangesAsync();
            return trackedEntity.Entity;
        }

        public async Task<IEnumerable<UserAccount>> GetBooks(Guid userId)
        {
            return await context.Set<UserAccount>()
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserAccount>> GetUsers(long bookId)
        {
            return await context
                .Set<UserAccount>()
                .Where(x => x.AccountId == bookId)
                .Include(x => x.User)
                .ToListAsync();
        }
    }
}
