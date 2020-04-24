﻿using Badzeet.Domain.Budget.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Domain.Budget.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Payment> Get(long id);
        Payment Add(Payment transaction);
        Task<Payment> Remove(long id);
        Task<IEnumerable<Payment>> GetPayments(long accountId, DateInterval interval);
        Task<Payment> GetLastPayment(long accountId);
        Task Save();
    }
}