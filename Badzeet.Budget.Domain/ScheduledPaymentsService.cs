using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Badzeet.Integration.Events;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain
{
    public class ScheduledPaymentsService : IRequestHandler<NewScheduledPaymentRequest>
    {
        private readonly IPaymentRepository repository;

        public ScheduledPaymentsService(IPaymentRepository paymentRepository)
        {
            this.repository = paymentRepository;
        }

        public Task<IEnumerable<Payment>> GetPayments(Guid userId, long accountId)
        {
            return repository.GetPayments(new PaymentsFilter(accountId, userId));
        }

        public Task<Unit> Handle(NewScheduledPaymentRequest request, CancellationToken cancellationToken)
        {
            repository.Add(new Payment(0, request.Date, request.Description, request.Amount, request.CategoryId, request.OwnerId, PaymentType.Scheduled, request.AccountId));
            return Task.FromResult(Unit.Value);
        }

        public async Task Transform(long id)
        {
            var scheduledPayment = await repository.Get(id);
            scheduledPayment.Type = PaymentType.Normal;
            await this.repository.Save();
        }
    }
}