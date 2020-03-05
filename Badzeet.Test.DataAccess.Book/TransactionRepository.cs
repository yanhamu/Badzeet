using Badzeet.Domain.Book;
using Badzeet.Domain.Book.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Test.DataAccess.Book
{
    public class TransactionRepository : ITransactionRepository
    {
        public async Task<Transaction> GetTransaction(long id)
        {
            var transactions = await GetTransactions(default, default, default);
            return transactions.SingleOrDefault(x => x.Id == id);
        }

        public Task<IEnumerable<Transaction>> GetTransactions(long accountId, DateTime from, DateTime to)
        {
            var date = from;
            var i = 1;

            var transactions = new List<Transaction>();
            while (date <= to)
            {
                transactions.Add(new Transaction(i, date, i.ToString(), i));
                date = date.AddDays(1);
                i += 1;
            }

            return Task.FromResult<IEnumerable<Transaction>>(transactions);
        }
    }
}