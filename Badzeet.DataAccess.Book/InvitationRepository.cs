﻿using Badzeet.DataAccess.Budget;
using Badzeet.Domain.Budget.Interfaces;
using Badzeet.Domain.Budget.Model;
using System;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Budget
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly BookDbContext context;

        public InvitationRepository(BookDbContext context)
        {
            this.context = context;
        }

        public async Task<Invitation> Create(Guid userId, long bookId)
        {
            var tracked = context.Set<Invitation>().Add(new Invitation() { Id = Guid.NewGuid(), AccountId = bookId, OwnerId = userId, CreatedAt = DateTime.UtcNow });
            await context.SaveChangesAsync();
            return tracked.Entity;
        }

        public async Task<Invitation> Get(Guid id)
        {
            var tracked = await context
                .Set<Invitation>()
                .FindAsync(id);
            return tracked;
        }
    }
}