using Badzeet.Domain.Book.Interfaces;
using System;
using System.Threading.Tasks;

namespace Badzeet.Domain.Book
{
    public class BookService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IBookRepository bookRepository;

        public BookService(
            ITransactionRepository transactionRepository,
            IBookRepository bookRepository)
        {
            this.transactionRepository = transactionRepository;
            this.bookRepository = bookRepository;
        }

        public async Task<DateInterval> GetLatestBudget(long bookId)
        {
            var book = await bookRepository.GetBook(bookId);
            var transaction = await transactionRepository.GetLastTransaction(bookId);
            if (transaction is null)
                return default;

            return GetBudgetInterval(book, transaction.Date);
        }

        private DateInterval GetBudgetInterval(Model.Book book, DateTime date)
        {
            DateTime startDate;
            if (date.Day >= book.FirstDayOfTheBudget)
            {
                startDate = new DateTime(date.Year, date.Month, book.FirstDayOfTheBudget);
            }
            else
            {
                startDate = new DateTime(date.AddMonths(-1).Year, date.AddMonths(-1).Month, book.FirstDayOfTheBudget);
            }

            var endDate = startDate.AddMonths(1).AddDays(-1);

            return new DateInterval(startDate, endDate);
        }

    }

}