using Badzeet.Domain.Book;
using Badzeet.Domain.Book.Interfaces;
using Badzeet.Web.Configuration;
using Badzeet.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
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
                var allCategories = await categoryRepository.GetCategories(bookId);
                var transactions = await transactionRepository.GetTransactions(bookId, interval);
                var allUsers = await userBookRepository.GetUsers(bookId);

                var categories = new List<CategoryViewModel>();
                var total = 0m;
                var users = new Dictionary<Guid, UserViewModel>();

                foreach (var category in allCategories)
                {
                    var categoryTransactions = transactions.Where(x => x.CategoryId == category.Id).ToArray();
                    var totalSum = 0m;
                    var perUserSum = new Dictionary<Guid, decimal>();
                    foreach (var user in allUsers)
                    {
                        var sum = categoryTransactions
                            .Where(x => x.UserId == user.UserId)
                            .Sum(x => x.Amount);

                        perUserSum.Add(user.UserId, sum);

                        totalSum += sum;
                        total += sum;

                        if (users.ContainsKey(user.UserId) == false)
                            users.Add(user.UserId, new UserViewModel(user.UserId, user.Nickname, 0));

                        users[user.UserId].Total += sum;
                    }
                    var c = new CategoryViewModel(category.Id, category.Name, perUserSum, totalSum);
                    categories.Add(c);
                }

                var model = new DashboardViewModel(
                    interval,
                    categories,
                    users,
                    total);
                return View(model);
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
        public CategoryViewModel(
            long id,
            string name,
            IDictionary<Guid, decimal> perUserSum,
            decimal sum)
        {
            Id = id;
            Name = name;
            PerUserSum = perUserSum;
            Sum = sum;
        }

        public long Id { get; }
        public string Name { get; }

        public decimal Sum { get; }
        public IDictionary<Guid, decimal> PerUserSum { get; }
    }

    public class UserViewModel
    {
        public UserViewModel(Guid id, string nickname, decimal total)
        {
            Id = id;
            Nickname = nickname;
            Total = total;
        }

        public Guid Id { get; }
        public string Nickname { get; }
        public decimal Total { get; set; }
    }

    public class DashboardViewModel
    {
        public DateInterval Interval { get; set; }
        public IEnumerable<CategoryViewModel> Categories { get; set; }
        public IDictionary<Guid, UserViewModel> Users { get; set; }
        public decimal Total { get; set; }

        public DashboardViewModel(
            DateInterval interval,
            IEnumerable<CategoryViewModel> categories,
            IDictionary<Guid, UserViewModel> users,
            decimal total)
        {
            this.Interval = interval;
            this.Categories = categories;
            this.Users = users;
            this.Total = total;
        }
    }
}