using Badzeet.Domain.Book;
using Badzeet.Domain.Book.Interfaces;
using Badzeet.Domain.Book.Model;
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
        private readonly ITransactionRepository transactionRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserBookRepository userBookRepository;
        private readonly ICategoryBudgetRepository budgetRepository;
        private readonly BookService budgetService;

        public BudgetController(
            ITransactionRepository transactionRepository,
            ICategoryRepository categoryRepository,
            IUserBookRepository userBookRepository,
            ICategoryBudgetRepository budgetRepository,
            BookService budgetService)
        {
            this.transactionRepository = transactionRepository;
            this.categoryRepository = categoryRepository;
            this.userBookRepository = userBookRepository;
            this.budgetRepository = budgetRepository;
            this.budgetService = budgetService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(long accountId, int budgetId)
        {
            if (await budgetRepository.HasBudget(accountId, budgetId) == false)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            var allCategories = await categoryRepository.GetCategories(accountId);
            var categories = new List<BudgetCategoryViewModel>();
            var interval = await budgetService.GetMonthlyBudgetById(accountId, budgetId);
            var transactions = await transactionRepository.GetTransactions(accountId, interval);
            var allUsers = await userBookRepository.GetUsers(accountId);
            var budgets = await budgetRepository.GetBudgets(accountId, budgetId);

            foreach (var c in allCategories)
            {
                var categoryTransactions = transactions.Where(x => x.CategoryId == c.Id);
                var budget = budgets.SingleOrDefault(x => x.CategoryId == c.Id);
                var users = new List<CategoryUserViewModel>();
                foreach (var u in allUsers)
                {
                    var total = categoryTransactions.Where(x => x.UserId == u.UserId).Sum(x => x.Amount);
                    users.Add(new CategoryUserViewModel(u.User.Nickname, total));
                }

                categories.Add(new BudgetCategoryViewModel()
                {
                    Name = c.Name,
                    Total = categoryTransactions.Sum(t => t.Amount),
                    Users = users.ToArray(),
                    Budget = budget?.Amount ?? 0
                });
            }

            var model = new BudgetViewModel();
            model.BudgetId = budgetId;
            model.Categories = categories.ToArray();
            model.Spend = categories.Sum(x => x.Total);
            model.Budget = categories.Sum(x => x.Budget);
            model.BudgetInterval = interval;
            return View(model);
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
    }
}
