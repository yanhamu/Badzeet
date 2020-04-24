using Badzeet.Domain.Budget.Interfaces;
using Badzeet.Domain.Budget.Model;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Budget
{
    public class AccountRepository : IAccountRepository
    {
        private readonly BookDbContext dbContext;

        public AccountRepository(BookDbContext dbContext)
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

        public async Task<Account> GetAccount(long accountId)
        {
            return await dbContext
                .Set<Account>()
                .FindAsync(accountId);
        }
    }
}
