using Badzeet.Scheduler.Domain.Interfaces;
using Badzeet.Scheduler.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Scheduler.DataAccess
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly SchedulerDbContext context;

        public PaymentRepository(SchedulerDbContext context)
        {
            this.context = context;
        }

        public void Add(Payment payment)
        {
            context.Set<Payment>().Add(payment);
        }

        public Task<List<Payment>> FetchAllToProcess(DateTime now)
        {
            return context.Set<Payment>()
                .Where(x => x.ScheduledAt <= now)
                .ToListAsync();
        }

        public async Task<Payment> Get(long id)
        {
            return await context.Set<Payment>().FindAsync(id);
        }

        public async Task<IEnumerable<Payment>> GetPayments(long accountId)
        {
            return await context.Set<Payment>()
                .Where(x => x.AccountId == accountId)
                .ToListAsync();
        }

        public Task<int> SaveAll()
        {
            return context.SaveChangesAsync();
        }
    }
}