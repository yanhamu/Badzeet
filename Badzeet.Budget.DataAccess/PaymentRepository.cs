using Badzeet.Budget.Domain;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Budget
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly BudgetDbContext context;

        public PaymentRepository(BudgetDbContext context)
        {
            this.context = context;
        }

        public Task<Payment> Get(long id)
        {
            return context.Set<Payment>().SingleOrDefaultAsync(x => x.Id == id);
        }

        public Payment Add(Payment payment)
        {
            return context.Set<Payment>().Add(payment).Entity;
        }

        public async Task<Payment> Remove(long id)
        {
            var payment = await context.Set<Payment>().FindAsync(id);
            context.Set<Payment>().Remove(payment);
            return payment;
        }

        public Task<Payment> GetLastPayment(long accountId)
        {
            return context
                .Set<Payment>()
                .Where(x => x.AccountId == accountId)
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync();
        }


        public async Task<IEnumerable<Payment>> GetPayments(long accountId, DateInterval interval)
        {
            return await context
                .Set<Payment>()
                .Where(x => x.AccountId == accountId)
                .Where(x => x.Date >= interval.From)
                .Where(x => x.Date <= interval.To)
                .OrderByDescending(x => x.Date)
                .Take(100)
                .ToListAsync();
        }

        public Task Save()
        {
            return context.SaveChangesAsync();
        }
    }
}