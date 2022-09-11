using Badzeet.Budget.DataAccess;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using System;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Budget
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BudgetDbContext dbContext;

        public AccountRepository(BudgetDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Account> CreateAccount(byte firstDayOfBudget)
        {
            var book = new Account() { FirstDayOfTheBudget = firstDayOfBudget };
            var bookEntity = dbContext
                .Set<Account>()
                .Add(book);
            await dbContext.SaveChangesAsync();
            return bookEntity.Entity;
        }

        public async Task<Account?> GetAccount(Guid accountId)
        {
            return await dbContext
                .Set<Account>()
                .FindAsync(accountId);
        }
    }
}
