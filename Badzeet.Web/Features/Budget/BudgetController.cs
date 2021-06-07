﻿using Badzeet.Budget.Domain;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Badzeet.Web.Features.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Budget
{
    [Authorize]
    public class BudgetController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IBudgetCategoryRepository budgetCategoryRepository;
        private readonly BudgetService budgetService;
        private readonly IBudgetRepository budgetRepository;
        private readonly IAccountRepository accountRepository;
        private readonly BudgetNavigationService budgetNavigation;

        public BudgetController(
            ICategoryRepository categoryRepository,
            IBudgetCategoryRepository budgetCategoryRepository,
            BudgetService budgetService,
            IBudgetRepository budgetRepository,
            IAccountRepository accountRepository,
            BudgetNavigationService budgetNavigation)
        {
            this.categoryRepository = categoryRepository;
            this.budgetCategoryRepository = budgetCategoryRepository;
            this.budgetService = budgetService;
            this.budgetRepository = budgetRepository;
            this.accountRepository = accountRepository;
            this.budgetNavigation = budgetNavigation;
        }

        [HttpGet]
        public async Task<IActionResult> Index(long accountId, long budgetId)
        {
            var budget = await budgetRepository.Get(budgetId);
            var navigation = await budgetNavigation.Get(accountId, budget.Date);

            return View(new BudgetViewModel(navigation));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long accountId, long budgetId)
        {
            var categories = await categoryRepository.GetCategories(accountId);
            var budgets = await budgetCategoryRepository.GetBudgetCategories(budgetId);

            var categoryBudgetModels = new List<CategoryBudgetViewModel>();
            foreach (var category in categories)
            {
                var v = new CategoryBudgetViewModel() { Amount = 0, CategoryId = category.Id, CategoryName = category.Name };
                var budget = budgets.SingleOrDefault(x => x.CategoryId == category.Id);

                if (budget != default)
                    v.Amount = budget.Amount;
                categoryBudgetModels.Add(v);
            }

            return View(new EditBudgetViewModel() { Budgets = categoryBudgetModels, BudgetId = budgetId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(long accountId, DateTime from)
        {
            var account = await accountRepository.GetAccount(accountId);
            var firstDayOgBudget = new DateTime(from.Year, from.Month, account.FirstDayOfTheBudget);
            var budget = budgetRepository.Create(new Badzeet.Budget.Domain.Model.Budget() { AccountId = account.Id, Date = firstDayOgBudget });
            await budgetRepository.Save();
            return RedirectToAction(nameof(Edit), new { accountId = accountId, budgetId = budget.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(long accountId, int budgetId, List<CategoryBudgetViewModel> budgets) //TODO remove accountId i guess
        {
            var tracked = await budgetCategoryRepository.GetBudgetCategories(budgetId);

            foreach (var t in tracked)
                t.Amount = budgets.Single(b => b.CategoryId == t.CategoryId).Amount;

            foreach (var b in budgets.Where(b => false == tracked.Any(t => t.CategoryId == b.CategoryId)))
                budgetCategoryRepository.AddBudget(new BudgetCategory() { BudgetId = budgetId, Amount = b.Amount, CategoryId = b.CategoryId });

            await budgetCategoryRepository.Save();

            return RedirectToAction(nameof(Index));
        }

        public class BudgetViewModel
        {
            public long BudgetId { get => BudgetNavigation.Current.BudgetId.Value; }
            public DateInterval BudgetInterval { get => new DateInterval(BudgetNavigation.Current.FirstBudgetDate, BudgetNavigation.Current.FirstBudgetDate.AddMonths(1).AddTicks(-1)); }
            public BudgetNavigationViewModel BudgetNavigation { get; set; }
            public BudgetViewModel(BudgetNavigationViewModel budgetNavigation)
            {
                this.BudgetNavigation = budgetNavigation;
            }
        }
    }
}
