using Badzeet.Domain.Book;
using Badzeet.Domain.Book.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Dashboard
{
    public class DashboardController : Controller
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserBookRepository userBookRepository;
        private readonly BookService budgetService;

        public DashboardController(
            ITransactionRepository transactionRepository,
            ICategoryRepository categoryRepository,
            IUserBookRepository userBookRepository,
            BookService budgetService)
        {
            this.transactionRepository = transactionRepository;
            this.categoryRepository = categoryRepository;
            this.userBookRepository = userBookRepository;
            this.budgetService = budgetService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(long bookId, int budgetId)
        {
            var interval = await budgetService.GetBudgetByOffset(bookId, budgetId);
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
                        users.Add(user.UserId, new UserViewModel(user.UserId, user.User.Nickname, 0));

                    users[user.UserId].Total += sum;
                }
                var c = new CategoryViewModel(category.Id, category.Name, perUserSum, totalSum);
                categories.Add(c);
            }

            var model = new DashboardViewModel(
                budgetId,
                interval,
                categories,
                users,
                total);
            return View(model);
        }
    }
}