using Badzeet.Domain.Book.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Domain.Book
{
    public class BookService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly IBookRepository bookRepository;
        private readonly ICategoryRepository categoryRepository;

        public BookService(
            ITransactionRepository transactionRepository,
            IBookRepository bookRepository,
            ICategoryRepository categoryRepository)
        {
            this.transactionRepository = transactionRepository;
            this.bookRepository = bookRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task<DateInterval> GetLatestBudget(long bookId)
        {
            var book = await bookRepository.GetBook(bookId);
            var transaction = await transactionRepository.GetLastTransaction(bookId);
            if (transaction is null)
                return default;

            return GetBudgetInterval(book, transaction.Date);
        }

        public async Task<IEnumerable<CategoryTuple>> GetBudget(long bookId, DateInterval interval)
        {
            var categories = await categoryRepository.GetCategories(bookId);
            var transactions = await transactionRepository.GetTransactions(bookId, interval);

            var categoryDict = categories.ToDictionary(k => k.Id, v => v.Name);
            return transactions
                .GroupBy(k => k.CategoryId, v => v.Amount)
                .Select(x => new CategoryTuple(x.Key ?? default, GetCategoryName(categoryDict, x.Key), x.Sum()));
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

        private string GetCategoryName(IDictionary<long, string> categories, long? id)
        {
            if (id.HasValue == false)
                return string.Empty;

            return categories.ContainsKey(id.Value)
                ? categories[id.Value]
                : string.Empty;
        }
    }

    public class CategoryTuple
    {
        public CategoryTuple(long id, string name, decimal amount)
        {
            Id = id;
            Name = name;
            Amount = amount;
        }

        public long Id { get; }
        public string Name { get; }
        public decimal Amount { get; }
    }
}