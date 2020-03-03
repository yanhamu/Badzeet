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
            return Task.FromResult<IEnumerable<Transaction>>(
                new List<Transaction>() {
                    new Transaction(9,new DateTime(2020,1,8), "Lunch", 165m),
                    new Transaction(8,new DateTime(2020,1,7), "Billa", 687),
                    new Transaction(7,new DateTime(2020,1,6), "Meat", 98m),
                    new Transaction(6,new DateTime(2020,1,5), "Lunch", 160m),
                    new Transaction(5,new DateTime(2020,1,4), "Pills", 416m),
                    new Transaction(4,new DateTime(2020,1,3), "Lunch", 220m),
                    new Transaction(3,new DateTime(2020,1,2), "Dinner", 180),
                    new Transaction(2,new DateTime(2020,1,2), "Chewing gum", 16),
                    new Transaction(1,new DateTime(2020,1,1), "Billa", 200m),
            });
        }
    }
}
