using Badzeet.Budget.DataAccess;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Budget
{
    public class UserAccountRepository : IUserAccountRepository
    {
        private readonly BudgetDbContext context;

        public UserAccountRepository(BudgetDbContext context)
        {
            this.context = context;
        }

        public async Task<UserAccount> Create(Guid userId, Guid accountId)
        {
            var userAccount = new UserAccount()
            {
                AccountId = accountId,
                UserId = userId
            };
            var trackedEntity = context.Set<UserAccount>()
                .Add(userAccount);
            await context.SaveChangesAsync();
            return trackedEntity.Entity;
        }

        public async Task<IEnumerable<UserAccount>> GetUserAccounts(Guid userId)
        {
            return await context.Set<UserAccount>()
                .Where(x => x.UserId == userId)
                .Include(x => x.User)
                .Include(x=>x.Account)
                .ToListAsync();
        }

        public async Task<IEnumerable<UserAccount>> GetUsers(Guid accountId)
        {
            return await context
                .Set<UserAccount>()
                .Where(x => x.AccountId == accountId)
                .Include(x => x.User)
                .ToListAsync();
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
