using Badzeet.Budget.Domain;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Budget
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly IPaymentRepository paymentsRepository;
        private readonly BudgetService budgetService;
        private readonly IBudgetCategoryRepository budgetCategoryRepository;
        private readonly IUserAccountRepository userAccountRepository;

        public SummaryViewComponent(
            IPaymentRepository paymentsRepository,
            BudgetService budgetService,
            IBudgetCategoryRepository budgetRepository,
            IUserAccountRepository userAccountRepository)
        {
            this.paymentsRepository = paymentsRepository;
            this.budgetService = budgetService;
            this.budgetCategoryRepository = budgetRepository;
            this.userAccountRepository = userAccountRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync(long accountId, long budgetId)
        {
            var interval = await budgetService.GetMonthlyBudgetById(budgetId);
            var normalPayments = await paymentsRepository.GetPayments(new PaymentsFilter(accountId, interval.From, interval.To, type: PaymentType.Normal));
            var pendingPayments = await paymentsRepository.GetPayments(new PaymentsFilter(accountId, interval.From, interval.To, type: PaymentType.Pending));
            var budgets = await budgetCategoryRepository.GetBudgetCategories(budgetId);
            var users = await userAccountRepository.GetUsers(accountId);

            var model = new SummaryViewModel()
            {
                Spend = normalPayments.Sum(x => x.Amount),
                Budget = budgets.Sum(x => x.Amount),
                BudgetInterval = interval,
                Pending = pendingPayments.Sum(x => x.Amount),
                Totals = normalPayments
                .GroupBy(x => new { x.UserId, users.Single(u => u.UserId == x.UserId).User.Nickname })
                .Select(x => new UserTotal(x.Key.UserId, x.Key.Nickname, x.Sum(y => y.Amount)))
            };

            return View(model);
        }

        public class SummaryViewModel
        {
            public decimal Spend { get; set; }
            public decimal SpendPercent
            {
                get => Budget > 0
                        ? Spend / Budget
                        : 0;

            }
            public decimal Budget { get; set; }
            public decimal Pending { get; set; }
            public decimal RemainingBudget { get { return Budget - Spend; } }
            public decimal RemainingBudgetPercent
            {
                get => Budget > 0
                    ? RemainingBudget / Budget
                    : 0;
            }
            public DateInterval BudgetInterval { get; set; }
            public bool IsOngoing { get { return DateTime.Now.Date <= BudgetInterval.To && DateTime.Now.Date >= BudgetInterval.From; } }
            public IEnumerable<UserTotal> Totals { get; set; }
        }

        public class UserTotal
        {
            public UserTotal(Guid userId, string username, decimal total)
            {
                UserId = userId;
                Username = username;
                Total = total;
            }
            public Guid UserId { get; set; }
            public string Username { get; }
            public decimal Total { get; }
        }
    }
}
