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

        public async Task<int> GetLatestBudgetOffset(long bookId)
        {
            var book = await bookRepository.GetBook(bookId);
            var first = await GetBudgetByOffset(bookId, 0);

            var lastTransaction = await transactionRepository.GetLastTransaction(bookId);
            var last = GetBudgetInterval(book, lastTransaction.Date);

            var id = 0;
            var date = first.From;
            while (date < last.From)
            {
                date = date.AddMonths(1);
                id += 1;
            }

            return id;
        }

        public async Task<DateInterval> GetBudgetByOffset(long bookId, int offset)
        {
            var book = await bookRepository.GetBook(bookId);
            var startInterval = GetBudgetInterval(book, book.CreatedAt);
            return new DateInterval(startInterval.From.AddMonths(offset), startInterval.To.AddMonths(offset));
        }

        private DateInterval GetBudgetInterval(Model.Account book, DateTime date)
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