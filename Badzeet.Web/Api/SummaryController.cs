using System;
using System.Linq;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace Badzeet.Web.Api;

[Route("api/accounts/{accountId:long}/budgets/{budgetId:int}")]
public class SummaryController : ControllerBase
{
    private readonly IBudgetCategoryRepository budgetCategoryRepository;
    private readonly IBudgetRepository budgetRepository;
    private readonly IPaymentRepository paymentRepository;

    public SummaryController(
        IBudgetRepository budgetRepository,
        IPaymentRepository paymentRepository,
        IBudgetCategoryRepository budgetCategoryRepository)
    {
        this.budgetRepository = budgetRepository;
        this.paymentRepository = paymentRepository;
        this.budgetCategoryRepository = budgetCategoryRepository;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> Get(long accountId, int budgetId)
    {
        var budget = await budgetRepository.Get(budgetId, accountId);
        var interval = budget.Interval;

        var normalPayments = await paymentRepository.GetPayments(new PaymentsFilter(
            accountId,
            interval.From,
            interval.To,
            type: PaymentType.Normal));

        var pendingPayments = await paymentRepository.GetPayments(new PaymentsFilter(
            accountId,
            interval.From,
            interval.To,
            type: PaymentType.Pending));

        var budgets = await budgetCategoryRepository.GetBudgetCategories(budgetId, accountId);

        var totalBudget = budgets.Sum(x => x.Amount);
        var remainingDays = GetRemainingDays(interval.To, DateTime.UtcNow);
        var remainingDailyBudget = 0.0m;
        if (remainingDays > 0) remainingDailyBudget = totalBudget / remainingDays;

        var model = new SummaryDto
        {
            Budget = budgets.Sum(x => x.Amount),
            Spend = normalPayments.Sum(x => x.Amount),
            PendingPayments = pendingPayments.Sum(x => x.Amount),
            RemainingDays = remainingDays,
            RemainingDailyBudget = remainingDailyBudget
        };

        return Ok(model);
    }

    private int GetRemainingDays(DateTime to, DateTime utcNow)
    {
        if (utcNow > to)
            return 0;

        return (int)Math.Ceiling((to - utcNow).TotalDays);
    }

    public class SummaryDto
    {
        public decimal Spend { get; set; }
        public decimal Budget { get; set; }
        public decimal RemainingBudget => Budget - Spend;
        public decimal RemainingDailyBudget { get; set; }
        public decimal PendingPayments { get; set; }
        public int RemainingDays { get; set; }
    }
}