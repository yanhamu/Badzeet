using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Budget
{
    public class ScheduledPaymentRepository : IScheduledPaymentRepository
    {
        private readonly BudgetDbContext context;

        public ScheduledPaymentRepository(BudgetDbContext context)
        {
            this.context = context;
        }

        public async Task<ScheduledPayment> GetPayment(long id)
        {
            return await context.Set<ScheduledPayment>().FindAsync(id);
        }

        public Task<List<ScheduledPayment>> GetPayments(Guid userId)
        {
            return context.Set<ScheduledPayment>().Where(x => x.OwnerId == userId).ToListAsync();
        }

        public async Task<ScheduledPayment> Remove(long id)
        {
            var payment = await context.Set<ScheduledPayment>().FindAsync(id);
            context.Set<ScheduledPayment>().Remove(payment);
            return payment;
        }
    }
}