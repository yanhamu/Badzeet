﻿using Badzeet.Budget.Domain;
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
        private readonly ICategoryBudgetRepository budgetRepository;
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IPaymentRepository paymentsRepository;

        public BudgetViewComponent(IPaymentRepository paymentsRepository,
            ICategoryRepository categoryRepository,
            IUserAccountRepository userAccountRepository,
            ICategoryBudgetRepository budgetRepository,
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
            var interval = await budgetService.GetMonthlyBudgetById(accountId, budgetId);
            var transactions = await paymentsRepository.GetPayments(new PaymentsFilter(accountId, interval: interval, type: PaymentType.Normal));
            var allUsers = await userAccountRepository.GetUsers(accountId);
            var budgets = await budgetRepository.GetBudgets(accountId, budgetId);

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

            return View(categories);
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
