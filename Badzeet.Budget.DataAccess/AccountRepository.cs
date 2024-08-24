using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;

namespace Badzeet.Budget.DataAccess;

public class AccountRepository : IAccountRepository
{
    private readonly BudgetDbContext dbContext;

    public AccountRepository(BudgetDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Account> CreateAccount(byte firstDayOfBudget)
    {
        var book = new Account { FirstDayOfTheBudget = firstDayOfBudget };
        var bookEntity = dbContext
            .Set<Account>()
            .Add(book);
        await dbContext.SaveChangesAsync();
        return bookEntity.Entity;
    }

    public async Task<Account?> GetAccount(long accountId)
    {
        return await dbContext
            .Set<Account>()
            .FindAsync(accountId);
    }
}