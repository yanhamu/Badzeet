using Badzeet.Budget.Domain;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Budget
{
    public class BudgetViewComponent : ViewComponent
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly BudgetService budgetService;
        private readonly IBudgetCategoryRepository budgetRepository;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IPaymentRepository paymentsRepository;

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

        public async Task<IViewComponentResult> InvokeAsync(long accountId, long budgetId)
        {
            var allCategories = await categoryRepository.GetCategories(accountId);
            var categories = new List<BudgetCategoryViewModel>();
            var interval = await budgetService.GetMonthlyBudgetById(budgetId);
            var transactions = await paymentsRepository.GetPayments(new PaymentsFilter(accountId, interval.From, interval.To, type: PaymentType.Normal));
            var allUsers = await userAccountRepository.GetUsers(accountId);
            var budgets = await budgetRepository.GetBudgetCategories(budgetId);

            foreach (var c in allCategories)
            {
                var categoryTransactions = transactions.Where(x => x.CategoryId == c.Id);
                var budget = budgets.SingleOrDefault(x => x.CategoryId == c.Id);
                var users = new List<CategoryUserViewModel>();
                foreach (var u in allUsers)
                {
                    var total = categoryTransactions.Where(x => x.UserId == u.UserId).Sum(x => x.Amount);
                    users.Add(new CategoryUserViewModel(u.UserId, u.User.Nickname, total));
                }

                categories.Add(new BudgetCategoryViewModel()
                {
                    Name = c.Name,
                    Total = categoryTransactions.Sum(t => t.Amount),
                    Users = users.ToArray(),
                    Budget = budget?.Amount ?? 0
                });
            }



            return View(new BudgetViewModel() { BudgetId = budgetId, Categories = categories });
        }

        public class BudgetViewModel
        {
            public List<BudgetCategoryViewModel> Categories { get; set; }
            public long BudgetId { get; set; }
        }

        public class BudgetCategoryViewModel
        {
            public string Name { get; set; }
            public decimal Total { get; set; }
            public decimal Budget { get; set; }
            public CategoryUserViewModel[] Users { get; set; }
        }
    }
}
