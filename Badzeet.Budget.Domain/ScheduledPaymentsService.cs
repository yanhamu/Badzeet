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
        private readonly IPaymentRepository paymentRepository;

        public ScheduledPaymentsService(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public Task<IEnumerable<Payment>> GetPayments(Guid userId)
        {
            return paymentRepository.GetPayments(userId, PaymentType.Scheduled);
        }

        public Task<Unit> Handle(NewScheduledPaymentRequest request, CancellationToken cancellationToken)
        {
            paymentRepository.Add(new Payment(0, request.Date, request.Description, request.Amount, request.CategoryId, request.OwnerId, PaymentType.Scheduled, request.AccountId));
            return Task.FromResult(Unit.Value);
        }

        public async Task Transform(long id)
        {
            var scheduledPayment = await paymentRepository.Get(id);
            scheduledPayment.Type = PaymentType.Normal;
            await this.paymentRepository.Save();
        }
    }
}