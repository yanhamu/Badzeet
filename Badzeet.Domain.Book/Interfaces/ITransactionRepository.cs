using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Domain.Book.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetTransactions(long accountId, DateTime from, DateTime to);
        Task<Transaction> GetTransaction(long id);
    }
}
