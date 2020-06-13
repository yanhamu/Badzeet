using Badzeet.Budget.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> Get(long id);
        Payment Add(Payment transaction);
        Task<Payment> Remove(long id);
        Task<IEnumerable<Payment>> GetPayments(PaymentsFilter filter);
        Task<Payment> GetLastPayment(long accountId);
        Task Save();
    }

    public class PaymentsFilter
    {
        public PaymentsFilter(long accountId, Guid? userId = null, DateInterval? interval = null, PaymentType? type = null) : this(accountId, new long[0], userId, interval, type) { }

        public PaymentsFilter(long accountId, long[] categoryId, Guid? userId = null, DateInterval? interval = null, PaymentType? type = null)
        {
            AccountId = accountId;
            UserId = userId;
            Interval = interval;
            PaymentType = type;
            CategoryId = categoryId;
        }

        public long AccountId { get; }
        public Guid? UserId { get; }
        public DateInterval? Interval { get; }
        public PaymentType? PaymentType { get; }
        public long[] CategoryId { get; set; }
    }
}