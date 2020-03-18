using Badzeet.Domain.Book;
using Badzeet.Domain.Book.Interfaces;
using Badzeet.Domain.Book.Model;
using Badzeet.Web.Configuration;
using Badzeet.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Home
{
    public class HomeController : Controller
    {
        private readonly BookService budgetService;
        private readonly ICategoryRepository categoryRepository;
        private readonly ITransactionRepository transactionRepository;
        private readonly IUserBookRepository userBookRepository;

        public HomeController(BookService budgetService,
            ICategoryRepository categoryRepository,
            ITransactionRepository transactionRepository,
            IUserBookRepository userBookRepository)
        {
            this.budgetService = budgetService;
            this.categoryRepository = categoryRepository;
            this.transactionRepository = transactionRepository;
            this.userBookRepository = userBookRepository;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var bookId = HttpContext.GetBookId();
                var interval = await budgetService.GetLatestBudget(bookId);
                var categories = (await categoryRepository.GetCategories(bookId)).Select(c => new CategoryViewModel(c.Id, c.Name));
                var transactions = await transactionRepository.GetTransactions(bookId, interval);
                var users = await userBookRepository.GetUsers(bookId);

                return View(new DashboardViewModel(interval, categories, transactions, users));
            }
            else
            {
                return View();
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class CategoryViewModel
    {
        public CategoryViewModel(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public long Id { get; }
        public string Name { get; }
    }

    public class DashboardViewModel
    {
        public DateInterval Interval { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; }
        public IEnumerable<UserBook> Users { get; set; }

        public DashboardViewModel(
            DateInterval interval,
            IEnumerable<CategoryViewModel> categories,
            IEnumerable<Transaction> transactions,
            IEnumerable<UserBook> users)
        {
            this.Interval = interval;
            this.Categories = categories;
            this.Transactions = transactions;
            this.Users = users;
        }
    }
}