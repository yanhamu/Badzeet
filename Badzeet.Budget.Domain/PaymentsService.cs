using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain
{
    public class PaymentsService
    {
        private readonly IPaymentRepository repository;

        public PaymentsService(IPaymentRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<Payment>> GetPayments(long accountId, DateInterval interval)
        {
            return await repository.GetPayments(new PaymentsFilter(accountId, interval: interval));
        }

        public Task<Payment> GetPayment(long id)
        {
            return repository.Get(id);
        }

        public Task Add(Payment payment)
        {
            repository.Add(payment);
            return repository.Save();
        }

        public async Task Remove(long id)
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
            await repository.Save();
        }
    }
}