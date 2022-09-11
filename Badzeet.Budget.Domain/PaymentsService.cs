using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Badzeet.Integration.Events;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain
{
    public class PaymentsService : IRequestHandler<NewScheduledPaymentRequest>
    {
        private readonly IPaymentRepository repository;

        public PaymentsService(IPaymentRepository repository)
        {
            this.repository = repository;
        }

        public Task<Payment?> GetPayment(Guid id)
        {
            return repository.Get(id);
        }

        public Task Add(Payment payment)
        {
            repository.Add(payment);
            return repository.Save();
        }

        public async Task Remove(Guid id)
        {
            await repository.Remove(id);
            await repository.Save();
        }

        public async Task Save(Payment payment)
        {
            var tracked = await repository.Get(payment.Id);
            tracked.Date = payment.Date;
            tracked.Description = payment.Description;
            tracked.Amount = payment.Amount;
            tracked.CategoryId = payment.CategoryId;
            tracked.UserId = payment.UserId;
            tracked.Type = payment.Type;
            await repository.Save();
        }

        public async Task Transform(Guid id)
        {
            var scheduledPayment = await repository.Get(id);
            scheduledPayment.Date = DateTime.UtcNow;
            scheduledPayment.Type = PaymentType.Normal;
            await this.repository.Save();
        }

        public async Task<Unit> Handle(NewScheduledPaymentRequest request, CancellationToken cancellationToken)
        {
            repository.Add(new Payment(Guid.NewGuid(), request.Date, request.Description, request.Amount, request.CategoryId, request.OwnerId, PaymentType.Pending, request.AccountId));
            await repository.Save();
            return Unit.Value;
        }

        public async Task Split(Guid oldPaymentId, decimal oldAmount, string description, decimal newAmount, Guid categoryId, Guid ownerId)
        {
            var payment = await repository.Get(oldPaymentId);
            payment.Amount = oldAmount;
            var newPayment = new Payment(Guid.NewGuid(), payment.Date, description, newAmount, categoryId, ownerId, PaymentType.Normal, payment.AccountId);
            repository.Add(newPayment);
            await repository.Save();
        }
    }
}