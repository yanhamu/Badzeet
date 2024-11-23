using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Badzeet.Budget.Domain;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Badzeet.Web.Features.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Badzeet.Web.Features.Budget;

[Authorize]
public class BudgetController : Controller
{
    private readonly IAccountRepository accountRepository;
    private readonly IBudgetCategoryRepository budgetCategoryRepository;
    private readonly BudgetNavigationService budgetNavigation;
    private readonly IBudgetRepository budgetRepository;
    private readonly ICategoryRepository categoryRepository;

    public BudgetController(
        ICategoryRepository categoryRepository,
        IBudgetCategoryRepository budgetCategoryRepository,
        IBudgetRepository budgetRepository,
        IAccountRepository accountRepository,
        BudgetNavigationService budgetNavigation)
    {
        this.categoryRepository = categoryRepository;
        this.budgetCategoryRepository = budgetCategoryRepository;
        this.budgetRepository = budgetRepository;
        this.accountRepository = accountRepository;
        this.budgetNavigation = budgetNavigation;
    }

    [HttpGet]
    public async Task<IActionResult> Index(long accountId, int budgetId)
    {
        var budget = await budgetRepository.Get(budgetId, accountId);
        if (budget == null) return RedirectToAction("Index", "Dashboard", new { accountId, budgetId });
        var navigation = await budgetNavigation.Get(accountId, budgetId);

        return View(new BudgetViewModel(navigation));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(long accountId, int budgetId)
    {
        var categories = await categoryRepository.GetCategories(accountId);
        var budgets = await budgetCategoryRepository.GetBudgetCategories(budgetId, accountId);

        var categoryBudgetModels = new List<CategoryBudgetViewModel>();
        foreach (var category in categories)
        {
            var v = new CategoryBudgetViewModel { Amount = 0, CategoryId = category.Id, CategoryName = category.Name };
            var budget = budgets.SingleOrDefault(x => x.CategoryId == category.Id);

            if (budget != default)
                v.Amount = budget.Amount;
            categoryBudgetModels.Add(v);
        }

        return View(new EditBudgetViewModel { Budgets = categoryBudgetModels, BudgetId = budgetId });
    }

    [HttpGet]
    public async Task<IActionResult> NewBudget(long accountId, int budgetId)
    {
        var budgetIdDateInt = budgetId * 100 + 1;
        var date = DateOnly.ParseExact(budgetIdDateInt.ToString(), "yyyyMMdd");
        var previousBudget = int.Parse(date.AddMonths(-1).ToString("yyyyMM"));

        var categories = await categoryRepository.GetCategories(accountId);
        var budgets = await budgetCategoryRepository.GetBudgetCategories(previousBudget, accountId);

        var categoryBudgetModels = new List<ComparisonCategoryBudgetViewModel>();
        foreach (var category in categories)
        {
            var v = new ComparisonCategoryBudgetViewModel { Amount = 0, CategoryId = category.Id, CategoryName = category.Name };
            var budget = budgets.SingleOrDefault(x => x.CategoryId == category.Id);

            if (budget != default)
            {
                v.Amount = budget.Amount;
                v.OldAmount = budget.Amount;
            }

            categoryBudgetModels.Add(v);
        }

        var model = new NewBudgetViewModel(categoryBudgetModels, budgetId);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> NewBudget(long accountId, int budgetId, List<CategoryBudgetViewModel> budgets)
    {
        var tracked = await budgetCategoryRepository.GetBudgetCategories(budgetId, accountId);

        foreach (var t in tracked)
            t.Amount = budgets.Single(b => b.CategoryId == t.CategoryId).Amount;

        foreach (var b in budgets.Where(b => false == tracked.Any(t => t.CategoryId == b.CategoryId)))
            budgetCategoryRepository.AddBudget(new BudgetCategory { Id = Guid.NewGuid(), BudgetId = budgetId, AccountId = accountId, Amount = b.Amount, CategoryId = b.CategoryId });

        await budgetCategoryRepository.Save();

        return RedirectToAction(nameof(Index), new { budgetId });
    }

    [HttpPost]
    public async Task<IActionResult> Create(long accountId, DateTime from)
    {
        var account = await accountRepository.GetAccount(accountId);
        var firstDayOfBudget = new DateTime(from.Year, from.Month, account.FirstDayOfTheBudget);
        var budgetId = int.Parse(from.ToString("yyyyMM"));
        var budget = budgetRepository.Create(new Badzeet.Budget.Domain.Model.Budget
        {
            Id = Guid.NewGuid(), AccountId = account.Id, BudgetId = budgetId, DateFrom = firstDayOfBudget, DateTo = firstDayOfBudget.AddMonths(1).AddTicks(-1)
        });
        await budgetRepository.Save();
        return RedirectToAction(nameof(NewBudget), new { budgetId = budget.BudgetId });
    }

    [HttpPost]
    public async Task<IActionResult> Edit(long accountId, int budgetId, List<CategoryBudgetViewModel> budgets)
    {
        var tracked = await budgetCategoryRepository.GetBudgetCategories(budgetId, accountId);

        foreach (var t in tracked)
            t.Amount = budgets.Single(b => b.CategoryId == t.CategoryId).Amount;

        foreach (var b in budgets.Where(b => false == tracked.Any(t => t.CategoryId == b.CategoryId)))
            budgetCategoryRepository.AddBudget(new BudgetCategory { Id = Guid.NewGuid(), BudgetId = budgetId, AccountId = accountId, Amount = b.Amount, CategoryId = b.CategoryId });

        await budgetCategoryRepository.Save();

        return RedirectToAction(nameof(Index), new { budgetId });
    }

    public class BudgetViewModel
    {
        public BudgetViewModel(BudgetNavigationViewModel budgetNavigation)
        {
            BudgetNavigation = budgetNavigation;
        }

        public int BudgetId => BudgetNavigation.Current.BudgetId;
        public DateInterval BudgetInterval => new(BudgetNavigation.Current.FirstBudgetDate, BudgetNavigation.Current.FirstBudgetDate.AddMonths(1).AddTicks(-1));
        public BudgetNavigationViewModel BudgetNavigation { get; set; }
    }
}