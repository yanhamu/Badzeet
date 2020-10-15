using Badzeet.Budget.Domain;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Budget
{
    [Authorize]
    public class BudgetController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly ICategoryBudgetRepository budgetRepository;
        private readonly BudgetService budgetService;

        public BudgetController(
            ICategoryRepository categoryRepository,
            ICategoryBudgetRepository budgetRepository,
            BudgetService budgetService)
        {
            this.categoryRepository = categoryRepository;
            this.budgetRepository = budgetRepository;
            this.budgetService = budgetService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(long accountId, int budgetId)
        {
            if (await budgetRepository.HasBudget(accountId, budgetId) == false)
                return RedirectToAction("Index", "Dashboard");

            var interval = await budgetService.GetMonthlyBudgetById(accountId, budgetId);

            return View(new BudgetViewModel
            {
                BudgetId = budgetId,
                BudgetInterval = interval,
            });
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long accountId, int budgetId)
        {
            var categories = await categoryRepository.GetCategories(accountId);
            var budgets = await budgetRepository.GetBudgets(accountId, budgetId);

            var categoryBudgetModels = new List<CategoryBudgetViewModel>();
            foreach (var category in categories)
            {
                var v = new CategoryBudgetViewModel() { Amount = 0, CategoryId = category.Id, CategoryName = category.Name };
                var budget = budgets.SingleOrDefault(x => x.CategoryId == category.Id);

                if (budget != default)
                    v.Amount = budget.Amount;
                categoryBudgetModels.Add(v);
            }

            return View(new EditBudgetViewModel() { Budgets = categoryBudgetModels });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(long accountId, int budgetId, List<CategoryBudgetViewModel> budgets)
        {
            var tracked = await budgetRepository.GetBudgets(accountId, budgetId);

            foreach (var t in tracked)
                t.Amount = budgets.Single(b => b.CategoryId == t.CategoryId).Amount;

            foreach (var b in budgets.Where(b => false == tracked.Any(t => t.CategoryId == b.CategoryId)))
                budgetRepository.AddBudget(new CategoryBudget() { AccountId = accountId, Amount = b.Amount, CategoryId = b.CategoryId, Id = budgetId });

            await budgetRepository.Save();

            return RedirectToAction(nameof(Index));
        }

        public class BudgetViewModel
        {
            public int BudgetId { get; set; }
            public DateInterval BudgetInterval { get; set; }
        }
    }
}
