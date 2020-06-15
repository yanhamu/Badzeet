using Badzeet.Budget.Domain;
using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.UserAccounts
{
    public class UserAccountsViewComponent : ViewComponent
    {
        private readonly IUserAccountRepository userAccountRepository;
        private readonly IPaymentRepository paymentRepository;
        private readonly BudgetService budgetService;

        public UserAccountsViewComponent(
            IUserAccountRepository userAccountRepository,
            IPaymentRepository paymentRepository,
            BudgetService budgetService)
        {
            this.userAccountRepository = userAccountRepository;
            this.paymentRepository = paymentRepository;
            this.budgetService = budgetService;
        }

        public async Task<IViewComponentResult> InvokeAsync(long accountId, int budgetId)
        {
            var a = await userAccountRepository.GetUsers(accountId);
            var interval = await budgetService.GetMonthlyBudgetById(accountId, budgetId);
            var payments = await paymentRepository.GetPayments(new PaymentsFilter(accountId, interval: interval, type: Badzeet.Budget.Domain.Model.PaymentType.Normal));
            var accounts = a.Select(x => new UserAccount()
            {
                Balance = x.Balance,
                Expenses = payments.Where(y => y.UserId == x.UserId).Sum(y => y.Amount),
                FinalBalance = x.Balance - payments.Where(y => y.UserId == x.UserId).Sum(y => y.Amount),
                Name = x.User.Nickname,
                UserId = x.UserId
            }).ToList();

            return View(accounts);
        }
    }

    public class UserAccount
    {
        public Guid UserId { get; set; }
        public decimal? Balance { get; set; }
        public decimal Expenses { get; set; }
        public decimal? FinalBalance { get; set; }
        public string Name { get; set; }
    }
}
