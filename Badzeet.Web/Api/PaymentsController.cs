using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
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
            var paymentsFilter = new PaymentsFilter(accountId, null, null, filter.From, filter.To);
            var payments = await paymentRepository.GetPayments(paymentsFilter);
            var result = payments.Select(p => new PaymentDto(p));
            return Ok(result);
        }

        [HttpGet("budgets/{budgetId:long}/payments")]
        public async Task<IActionResult> List(long accountId, long budgetId)
        {
            var budget = await budgetRepository.Get(budgetId);
            var paymentsFilter = new PaymentsFilter(accountId, budget.Interval.From, budget.Interval.To, null, PaymentType.Normal);
            var payments = await paymentRepository.GetPayments(paymentsFilter);
            var result = payments.Select(p => new PaymentDto(p));
            return Ok(result);
        }

        [HttpPost("payments")]
        public async Task<IActionResult> Create(long accountId, [FromBody]NewPaymentDto payment)
        {
            var savedPayment = paymentRepository.Add(new Payment()
            {
                AccountId = accountId,
                CategoryId = payment.CategoryId,
                Amount = payment.Amount,
                Date = payment.Date,
                Description = payment.Description,
                Type = payment.Type,
                UserId = payment.UserId
            });

            await paymentRepository.Save();
            return Ok(savedPayment);
        }

        public class NewPaymentDto
        {
            public DateTime Date { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public long CategoryId { get; set; }
            public Guid UserId { get; set; }
            public PaymentType Type { get; set; }
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

        public class Filter
        {
            public DateTime? From { get; set; }
            public DateTime? To { get; set; }
        }
    }

}