﻿using Badzeet.Budget.DataAccess;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Budget
{
    public class UserBookRepository : IUserAccountRepository
    {
        private readonly BudgetDbContext context;

        public UserBookRepository(BudgetDbContext context)
        {
            this.context = context;
        }

        public async Task<UserAccount> Create(Guid userId, long accountId)
        {
            var userBook = new UserAccount()
            {
                AccountId = accountId,
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

        public async Task<IEnumerable<UserAccount>> GetUsers(long accountId)
        {
            return await context
                .Set<UserAccount>()
                .Where(x => x.AccountId == accountId)
                .Include(x => x.User)
                .ToListAsync();
        }
    }
}
