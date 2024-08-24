using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Badzeet.Scheduler.Domain.Model;

namespace Badzeet.Scheduler.Domain.Interfaces;

public interface IPaymentRepository
{
    Task<List<Payment>> FetchAllToProcess(DateTime now);
    Task<int> SaveAll();
    Task<IEnumerable<Payment>> GetPayments(long accountId);
    void Add(Payment payment);
    Task<Payment?> Get(long id);
    Task Remove(long id);
}