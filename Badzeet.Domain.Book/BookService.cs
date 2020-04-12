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

        public async Task<int> GetLatestBudgetId(long bookId)
        {
            var book = await bookRepository.GetBook(bookId);
            var lastTransaction = await transactionRepository.GetLastTransaction(bookId);
            var last = GetBudgetInterval(book.FirstDayOfTheBudget, lastTransaction.Date);

            var pivot = new DateTime(2000, 1, book.FirstDayOfTheBudget);
            var id = 0;
            while (pivot < last.From)
            {
                id += 1;
                pivot = pivot.AddMonths(1);
            }

            return id;
        }

        public async Task<DateInterval> GetMonthlyBudgetById(long bookId, int budgetId)
        {
            var book = await bookRepository.GetBook(bookId);
            var start = new DateTime(2000, 1, book.FirstDayOfTheBudget);
            return new DateInterval(start, start.AddMonths(1).AddDays(-1)).AddMonth(budgetId);
        }

        private DateInterval GetBudgetInterval(byte firstDay, DateTime date)
        {
            DateTime startDate;
            if (date.Day >= firstDay)
            {
                startDate = new DateTime(date.Year, date.Month, firstDay);
            }
            else
            {
                startDate = new DateTime(date.AddMonths(-1).Year, date.AddMonths(-1).Month, firstDay);
            }

            var endDate = startDate.AddMonths(1).AddDays(-1);

            return new DateInterval(startDate, endDate);
        }
    }
}