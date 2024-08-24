using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Badzeet.Budget.Domain;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace Badzeet.Web.Features.Budget;

public class BudgetViewComponent : ViewComponent
{
    private readonly IBudgetCategoryRepository budgetRepository;
    private readonly BudgetService budgetService;
    private readonly ICategoryRepository categoryRepository;
    private readonly IPaymentRepository paymentsRepository;
    private readonly IUserAccountRepository userAccountRepository;

    public BudgetViewComponent(IPaymentRepository paymentsRepository,
        ICategoryRepository categoryRepository,
        IUserAccountRepository userAccountRepository,
        IBudgetCategoryRepository budgetRepository,
        BudgetService budgetService)
    {
        this.categoryRepository = categoryRepository;
        this.budgetService = budgetService;
        this.budgetRepository = budgetRepository;
        this.userAccountRepository = userAccountRepository;
        this.paymentsRepository = paymentsRepository;
    }

    public async Task<IViewComponentResult> InvokeAsync(long accountId, int budgetId)
    {
        var allCategories = await categoryRepository.GetCategories(accountId);
        var categories = new List<BudgetCategoryViewModel>();
        var interval = await budgetService.GetMonthlyBudgetById(budgetId, accountId);
        var payments = await paymentsRepository.GetPayments(new PaymentsFilter(accountId, interval.From, interval.To, type: PaymentType.Normal));
        var allUsers = await userAccountRepository.GetUsers(accountId);
        var budgets = await budgetRepository.GetBudgetCategories(budgetId, accountId);

        foreach (var c in allCategories)
        {
            var categoryTransactions = payments.Where(x => x.CategoryId == c.Id);
            var budget = budgets.SingleOrDefault(x => x.CategoryId == c.Id);
            var users = new List<CategoryUserViewModel>();
            foreach (var u in allUsers)
            {
                var total = categoryTransactions.Where(x => x.UserId == u.UserId).Sum(x => x.Amount);
                users.Add(new CategoryUserViewModel(u.UserId, u.User.Nickname, total));
            }

            categories.Add(new BudgetCategoryViewModel
            {
                Name = c.Name,
                Total = categoryTransactions.Sum(t => t.Amount),
                Users = users.ToArray(),
                Budget = budget?.Amount ?? 0,
                Id = c.Id
            });
        }

        return View(new BudgetViewModel { BudgetId = budgetId, Categories = categories, Interval = interval });
    }

    public class BudgetViewModel
    {
        public List<BudgetCategoryViewModel> Categories { get; set; } = new();
        public long BudgetId { get; set; }
        public DateInterval Interval { get; set; }
    }

    public class BudgetCategoryViewModel
    {
        public string Name { get; set; } = default!;
        public decimal Total { get; set; }
        public decimal Budget { get; set; }
        public CategoryUserViewModel[] Users { get; set; }
        public long Id { get; set; }
    }
}