using Badzeet.Scheduler.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Scheduler.Domain.Interfaces
{
    public interface IPaymentRepository
    {
        Task<List<Payment>> FetchAllToProcess(DateTime now);
        Task SaveAll();
    }
}