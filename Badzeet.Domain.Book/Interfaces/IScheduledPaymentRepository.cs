using Badzeet.Domain.Budget.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Domain.Budget.Interfaces
{
    public interface IScheduledPaymentRepository
    {
        Task<List<ScheduledPayment>> GetScheduledPayments(Guid userId);
    }
}
