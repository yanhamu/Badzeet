using Badzeet.Scheduler.Domain.Interfaces;
using Badzeet.Scheduler.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Scheduler.Domain
{
    public class PaymentScheduler
    {
        private readonly IEnumerable<IProcessor> processors;
        private readonly IPaymentRepository repository;

        public PaymentScheduler(IEnumerable<IProcessor> processors, IPaymentRepository paymentRepository)
        {
            this.processors = processors;
            this.repository = paymentRepository;
        }

        public async Task Run()
        {
            var now = DateTime.UtcNow;
            var payments = await repository.FetchAllToProcess(now);

            foreach (var payment in payments)
            {
                var processor = processors.Single(x => x.Id == payment.SchedulingType);
                await processor.Process(payment, now);
            }
        }
    }

    public interface IProcessor
    {
        SchedulingType Id { get; }
        Task Process(Payment payment, DateTime now);
    }

    public class PaymentDto
    {
        public PaymentDto(long accountId, DateTime date, decimal amount, string description, long categoryId, Guid ownerId)
        {
            AccountId = accountId;
            Date = date;
            Amount = amount;
            Description = description;
            CategoryId = categoryId;
            OwnerId = ownerId;
        }

        public long AccountId { get; }
        public DateTime Date { get; }
        public decimal Amount { get; }
        public string Description { get; }
        public long CategoryId { get; }
        public Guid OwnerId { get; }
    }

    public interface IPaymentBus
    {
        Task Publish(PaymentDto paymentDto);
    }
}
