using Badzeet.Domain.Budget.Interfaces;
using Badzeet.Domain.Budget.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Domain.Budget
{
    public class ScheduledPaymentsService
    {
        private readonly IScheduledPaymentRepository repository;

        public ScheduledPaymentsService(IScheduledPaymentRepository repository)
        {
            this.repository = repository;
        }
        public Task<List<ScheduledPayment>> GetPayments(Guid userId)
        {
            return repository.GetScheduledPayments(userId);
        }
    }
}