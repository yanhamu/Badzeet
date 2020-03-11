using Badzeet.Domain.Book;
using Badzeet.Domain.Book.Interfaces;
using Badzeet.Domain.Book.Model;
using Microsoft.EntityFrameworkCore;
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

        public Transaction Add(Transaction transaction)
        {
            return context.Set<Transaction>().Add(transaction).Entity;
        }

        public Task<Transaction> GetLastTransaction(long bookId)
        {
            return context
                .Set<Transaction>()
                .Where(x => x.BookId == bookId)
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync();
        }

        public Task<Transaction> GetTransaction(long id)
        {
            return context.Set<Transaction>().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Transaction>> GetTransactions(long bookId, DateInterval interval)
        {
            return await context
                .Set<Transaction>()
                .Where(x => x.BookId == bookId)
                .Where(x => x.Date >= interval.From)
                .Where(x => x.Date <= interval.To)
                .OrderByDescending(x => x.Date)
                .Take(100)
                .ToListAsync();
        }

        public Task Save()
        {
            return context.SaveChangesAsync();
        }
    }
}