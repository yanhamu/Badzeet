using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Badzeet.Budget.Domain;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Web.Features.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Badzeet.Web.Features.Dashboard;

[Authorize]
public class DashboardController : Controller
{
    private readonly BudgetNavigationService budgetNavigationService;
    private readonly IBudgetRepository budgetRepository;
    private readonly ICategoryRepository categoryRepository;
    private readonly IPaymentRepository paymentsRepository;
    private readonly IUserAccountRepository userBookRepository;

    public DashboardController(
        IPaymentRepository paymentsRepository,
        ICategoryRepository categoryRepository,
        IUserAccountRepository userBookRepository,
        BudgetNavigationService budgetNavigationService,
        IBudgetRepository budgetRepository)
    {
        this.paymentsRepository = paymentsRepository;
        this.categoryRepository = categoryRepository;
        this.userBookRepository = userBookRepository;
        this.budgetNavigationService = budgetNavigationService;
        this.budgetRepository = budgetRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index(long accountId, int budgetId)
    {
        var budget = await budgetRepository.Get(budgetId, accountId);
        if (budget != null) return RedirectToAction("Index", "Budget", new { accountId, budgetId });

        var navigationDates = await budgetNavigationService.Get(accountId, budgetId);
        var interval = new DateInterval(navigationDates.Current.FirstBudgetDate, navigationDates.Current.FirstBudgetDate.AddMonths(1).AddTicks(-1));
        var allCategories = await categoryRepository.GetCategories(accountId);
        var transactions = await paymentsRepository.GetPayments(new PaymentsFilter(accountId, interval.From, interval.To));
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
            navigationDates,
            categories,
            users,
            total);
        return View(model);
    }
}