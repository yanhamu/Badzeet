using Badzeet.Domain.Book.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Domain.Book.Interfaces
{
    public interface IPaymentRepository
    {
        Task<IEnumerable<Payment>> GetPayments(long bookId, DateInterval interval);
        Task<Payment> GetPayment(long id);
        Task Save();
        Payment Add(Payment transaction);
        Task<Payment> GetLastPayment(long bookId);
        Task<Payment> Remove(long id);
    }
}
