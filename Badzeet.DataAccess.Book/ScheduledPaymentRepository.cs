using Badzeet.Domain.Budget.Interfaces;
using Badzeet.Domain.Budget.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Book
{
    public class ScheduledPaymentRepository : IScheduledPaymentRepository
    {
        private readonly BookDbContext context;

        public ScheduledPaymentRepository(BookDbContext context)
        {
            this.context = context;
        }
        public Task<List<ScheduledPayment>> GetScheduledPayments(Guid userId)
        {
            return context.Set<ScheduledPayment>().Where(x => x.OwnerId == userId).ToListAsync();
        }
    }
}