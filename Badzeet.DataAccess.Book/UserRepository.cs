using Badzeet.Domain.Budget.Interfaces;
using Badzeet.Domain.Budget.Model;
using System;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Budget
{
    public class UserRepository : IUserRepository
    {
        private readonly BookDbContext context;

        public UserRepository(BookDbContext context)
        {
            this.context = context;
        }
        public Task Create(Guid id, string username)
        {
            context.Set<User>().Add(new User() { Id = id, Nickname = username });
            return context.SaveChangesAsync();
        }
    }
}