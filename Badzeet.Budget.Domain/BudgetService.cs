using Badzeet.Budget.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain
{
    public class BudgetService
    {
        private readonly IPaymentRepository transactionRepository;
        private readonly IAccountRepository accountRepository;
        private readonly IBudgetRepository budgetRepository;

        public BudgetService(
            IPaymentRepository transactionRepository,
            IAccountRepository bookRepository,
            IBudgetRepository budgetRepository)
        {
            this.transactionRepository = transactionRepository;
            this.accountRepository = bookRepository;
            this.budgetRepository = budgetRepository;
        }

        public async Task<int> GetLatestBudgetId(Guid accountId)
        {
            var account = await accountRepository.GetAccount(accountId);
            var lastTransaction = await transactionRepository.GetLastPayment(accountId);
            var last = GetBudgetInterval(account.FirstDayOfTheBudget, lastTransaction.Date);

            var pivot = new DateTime(2000, 1, account.FirstDayOfTheBudget);
            short id = 0;
            while (pivot < last.From)
            {
                id += 1;
                pivot = pivot.AddMonths(1);
            }

            return id;
        }

        public async Task<DateInterval> GetMonthlyBudgetById(int budgetId, Guid accountId) //TODO remove
        {
            var budget = await budgetRepository.Get(budgetId, accountId);
            return budget.Interval;
        }

        public DateInterval GetBudgetInterval(byte firstDay, DateTime date)
        {
            DateTime startDate;
            if (date.Day >= firstDay)
            {
                startDate = new DateTime(date.Year, date.Month, firstDay);
            }
            else
            {
                startDate = new DateTime(date.AddMonths(-1).Year, date.AddMonths(-1).Month, firstDay);
            }

            var endDate = startDate.AddMonths(1).AddDays(-1);

            return new DateInterval(startDate, endDate);
        }
    }
}