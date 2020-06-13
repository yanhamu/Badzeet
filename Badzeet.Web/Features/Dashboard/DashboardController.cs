using Badzeet.Budget.Domain;
using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Dashboard
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly IPaymentRepository paymentsRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserAccountRepository userBookRepository;
        private readonly ICategoryBudgetRepository budgetRepository;
        private readonly BudgetService budgetService;

        public DashboardController(
            IPaymentRepository paymentsRepository,
            ICategoryRepository categoryRepository,
            IUserAccountRepository userBookRepository,
            ICategoryBudgetRepository budgetRepository,
            BudgetService budgetService)
        {
            this.paymentsRepository = paymentsRepository;
            this.categoryRepository = categoryRepository;
            this.userBookRepository = userBookRepository;
            this.budgetRepository = budgetRepository;
            this.budgetService = budgetService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(long accountId, int budgetId)
        {
            if (await budgetRepository.HasBudget(accountId, budgetId))
                return RedirectToAction("Index", "Budget");

            var interval = await budgetService.GetMonthlyBudgetById(accountId, budgetId);
            var allCategories = await categoryRepository.GetCategories(accountId);
            var transactions = await paymentsRepository.GetPayments(new PaymentsFilter(accountId, interval: interval));
            var allUsers = await userBookRepository.GetUsers(accountId);

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