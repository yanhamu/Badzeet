﻿using Badzeet.Domain.Book.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Domain.Book.Interfaces
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetTransactions(long bookId, DateInterval interval);
        Task<Transaction> GetTransaction(long id);
        Task Save();
        Transaction Add(Transaction transaction);
        Task<Transaction> GetLastTransaction(long bookId);
        Task<Transaction> Remove(long id);
    }
}
