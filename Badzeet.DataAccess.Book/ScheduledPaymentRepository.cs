using Badzeet.Domain.Budget.Interfaces;
using Badzeet.Domain.Budget.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Book
{
    public class ScheduledPaymentRepository : IScheduledPaymentRepository
    {
        public Task<List<ScheduledPayment>> GetScheduledPayments(Guid userId)
        {
            var result = new List<ScheduledPayment>()
            {
            };
            return Task.FromResult(result);
        }
    }
}
