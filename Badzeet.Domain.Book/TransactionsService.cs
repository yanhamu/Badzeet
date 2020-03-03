using Badzeet.Domain.Book.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Domain.Book.Model
{
    public class TransactionsService
    {
        private readonly ITransactionRepository repository;

        public TransactionsService(ITransactionRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IEnumerable<Transaction>> GetTransactions(long accountId, DateTime from, DateTime to)
        {
            return await repository.GetTransactions(accountId, from, to);
        }
    }
}
