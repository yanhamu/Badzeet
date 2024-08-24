using System;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;

namespace Badzeet.Budget.DataAccess;

public class UserRepository : IUserRepository
{
    private readonly BudgetDbContext context;

    public UserRepository(BudgetDbContext context)
    {
        this.context = context;
    }

    public Task Create(Guid id, string username)
    {
        context.Set<User>().Add(new User { Id = id, Nickname = username });
        return context.SaveChangesAsync();
    }
}