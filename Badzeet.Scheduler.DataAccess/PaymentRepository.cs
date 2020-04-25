using Badzeet.Scheduler.Domain.Interfaces;
using Badzeet.Scheduler.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Scheduler.DataAccess
{
    public class PaymentRepository : IPaymentRepository
    {
        public Task<List<Payment>> FetchAllToProcess(DateTime now)
        {
            //TODO implement
            var result = new List<Payment>();
            return Task.FromResult(result);
        }

        public Task SaveAll()
        {
            //TODO implement
            return Task.CompletedTask;
        }
    }
}
