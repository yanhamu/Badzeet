using Badzeet.Domain.Budget.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Domain.Budget.Interfaces
{
    public interface IScheduledPaymentRepository
    {
        Task<List<ScheduledPayment>> GetPayments(Guid userId);
        Task<ScheduledPayment> GetPayment(long id);
        Task<ScheduledPayment> Remove(long id);
    }
}
