using Badzeet.Budget.Domain;
using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Api
{
    [Route("api/accounts/{accountId:long}")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository paymentRepository;
        private readonly IBudgetRepository budgetRepository;

        public PaymentsController(IPaymentRepository paymentRepository, IBudgetRepository budgetRepository)
        {
            this.paymentRepository = paymentRepository;
            this.budgetRepository = budgetRepository;
        }

        [HttpGet("payments")]
        public async Task<IActionResult> List(long accountId, Filter filter)
        {
            var paymentsFilter = new PaymentsFilter(accountId, null, filter.Interval, Budget.Domain.Model.PaymentType.Normal);
            var payments = await paymentRepository.GetPayments(paymentsFilter);
            var result = payments.Select(p => new PaymentDto(p));
            return Ok(result);
        }

        [HttpGet("budgets/{budgetId:long}/payments")]
        public async Task<IActionResult> List(long accountId, long budgetId)
        {
            var budget = await budgetRepository.Get(budgetId);
            var paymentsFilter = new PaymentsFilter(accountId, null, budget.Interval, Budget.Domain.Model.PaymentType.Normal);
            var payments = await paymentRepository.GetPayments(paymentsFilter);
            var result = payments.Select(p => new PaymentDto(p));
            return Ok(result);
        }

        public class PaymentDto
        {
            public PaymentDto(Budget.Domain.Model.Payment payment)
            {
                this.Id = payment.Id;
                this.AccountId = payment.AccountId;
                this.Date = payment.Date;
                this.Description = payment.Description;
                this.CategoryId = payment.CategoryId;
                this.UserId = payment.UserId;
                this.Amount = payment.Amount;
            }

            public long Id { get; set; }
            public long AccountId { get; set; }
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public long CategoryId { get; set; }
            public Guid UserId { get; set; }
        }
    }

    public class Filter
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateInterval Interval { get => new DateInterval(From, To); }
    }
}