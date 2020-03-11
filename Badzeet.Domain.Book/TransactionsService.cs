using Badzeet.Domain.Book.Interfaces;
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

        public async Task<IEnumerable<Transaction>> GetTransactions(long accountId, DateInterval interval)
        {
            return await repository.GetTransactions(accountId, interval);
        }

        public async Task Save(Transaction transaction)
        {
            var tracked = await repository.GetTransaction(transaction.Id);
            tracked.Date = transaction.Date;
            tracked.Description = transaction.Description;
            tracked.Amount = transaction.Amount;
            tracked.CategoryId = transaction.CategoryId;
            tracked.UserId = transaction.UserId;
            await repository.Save();
        }
    }
}