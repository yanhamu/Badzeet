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
        private readonly ICategoryRepository categoryRepository;

        public PaymentsService(IPaymentRepository repository, ICategoryRepository categoryRepository)
        {
            this.repository = repository;
            this.categoryRepository = categoryRepository;
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
            tracked.Category = payment.Category;
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
            var category = await categoryRepository.Get(request.CategoryId);
            repository.Add(new Payment(Guid.NewGuid(), request.Date, request.Description, request.Amount, category.Name, request.OwnerId, PaymentType.Pending, request.AccountId));
            await repository.Save();
            return Unit.Value;
        }

        public async Task Split(Guid oldPaymentId, decimal oldAmount, string description, decimal newAmount, string category, Guid ownerId)
        {
            var payment = await repository.Get(oldPaymentId);
            payment.Amount = oldAmount;
            var newPayment = new Payment(Guid.NewGuid(), payment.Date, description, newAmount, category, ownerId, PaymentType.Normal, payment.AccountId);
            repository.Add(newPayment);
            await repository.Save();
        }
    }
}