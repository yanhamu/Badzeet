using Badzeet.Domain.Book.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Domain.Book.Model
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
            return await repository.GetPayments(accountId, interval);
        }

        public async Task Save(Payment payment)
        {
            var tracked = await repository.GetPayment(payment.Id);
            tracked.Date = payment.Date;
            tracked.Description = payment.Description;
            tracked.Amount = payment.Amount;
            tracked.CategoryId = payment.CategoryId;
            tracked.UserId = payment.UserId;
            await repository.Save();
        }
    }
}