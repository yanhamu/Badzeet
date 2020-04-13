using Badzeet.Domain.Book.Interfaces;
using Badzeet.Domain.Book.Model;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Book
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
