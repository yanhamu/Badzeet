using Badzeet.Budget.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain.Interfaces
{
    public interface IScheduledPaymentRepository
    {
        Task<List<ScheduledPayment>> GetPayments(Guid userId);
        Task<ScheduledPayment> GetPayment(long id);
        Task<ScheduledPayment> Remove(long id);
    }
}
