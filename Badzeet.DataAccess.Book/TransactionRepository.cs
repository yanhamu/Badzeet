using Badzeet.Domain.Book;
using Badzeet.Domain.Book.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Book
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly BookDbContext context;

        public TransactionRepository(BookDbContext context)
        {
            this.context = context;
        }

        public Task<Transaction> GetTransaction(long id)
        {
            return context.Set<Transaction>().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Transaction>> GetTransactions(long accountId, DateTime from, DateTime to)
        {
            return await context
                .Set<Transaction>()
                .Where(x => x.Date >= from)
                .Where(x => x.Date <= to)
                .ToListAsync();
        }

        public Task Save()
        {
            return context.SaveChangesAsync();
        }
    }
}