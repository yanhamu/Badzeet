using Badzeet.Domain.Book.Interfaces;
using Badzeet.Domain.Book.Model;
using System;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Book
{
    public class UserRepository : IUserRepository
    {
        private readonly BookDbContext context;

        public UserRepository(BookDbContext context)
        {
            this.context = context;
        }
        public Task Create(Guid id)
        {
            context.Set<User>().Add(new User() { Id = id });
            return context.SaveChangesAsync();
        }
    }
}