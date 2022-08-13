using Badzeet.Budget.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> Get(long id);
        Payment Add(Payment transaction);
        Task<Payment> Remove(long id);
        Task<IEnumerable<Payment>> GetPayments(PaymentsFilter filter);
        Task<Payment?> GetLastPayment(long accountId);
        Task Save();
    }

    public class PaymentsFilter
    {
        public PaymentsFilter(
            long accountId,
            DateTime? from,
            DateTime? to,
            Guid? userId = null,
            PaymentType? type = null) : this(accountId, Array.Empty<long>(), userId, from, to, type) { }

        public PaymentsFilter(
            long accountId,
            long[] categoryId,
            Guid? userId = null,
            DateTime? from = null,
            DateTime? to = null,
            PaymentType? type = null)
        {
            AccountId = accountId;
            UserId = userId;
            Interval = new OpenDateInterval(from, to);
            PaymentType = type;
            CategoryId = categoryId;
        }

        public long AccountId { get; }
        public Guid? UserId { get; }
        public OpenDateInterval Interval { get; }
        public PaymentType? PaymentType { get; }
        public long[] CategoryId { get; set; }
    }
}