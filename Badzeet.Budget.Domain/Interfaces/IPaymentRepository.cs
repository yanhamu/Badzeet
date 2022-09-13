using Badzeet.Budget.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment?> Get(Guid id);
        Payment Add(Payment transaction);
        Task<Payment?> Remove(Guid id);
        Task<IEnumerable<Payment>> GetPayments(PaymentsFilter filter);
        Task<Payment?> GetLastPayment(Guid accountId);
        Task Save();
    }

    public class PaymentsFilter
    {
        public PaymentsFilter(
            Guid accountId,
            DateTime? from,
            DateTime? to,
            Guid? userId = null,
            PaymentType? type = null) : this(accountId, Array.Empty<string>(), userId, from, to, type) { }

        public PaymentsFilter(
            Guid accountId,
            string[] categories,
            Guid? userId = null,
            DateTime? from = null,
            DateTime? to = null,
            PaymentType? type = null)
        {
            AccountId = accountId;
            UserId = userId;
            Interval = new OpenDateInterval(from, to);
            PaymentType = type;
            Categories = categories;
        }

        public Guid AccountId { get; }
        public Guid? UserId { get; }
        public OpenDateInterval Interval { get; }
        public PaymentType? PaymentType { get; }
        public string[] Categories { get; set; } = Array.Empty<string>();
    }
}