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

        public PaymentsController(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        [HttpGet("payments")]
        public async Task<IActionResult> List(long accountId, Filter filter)
        {
            var paymentsFilter = new PaymentsFilter(accountId, Array.Empty<long>(), null, filter.From.ToDateTime(), filter.To.ToDateTime(), filter.Type);
            var payments = await paymentRepository.GetPayments(paymentsFilter);
            var result = payments.Select(p => new PaymentDto(p));
            return Ok(result);
        }

        [HttpGet("payments/{paymentId:long}")]
        public async Task<IActionResult> Get(long accountId, long paymentId)
        {
            var payment = await paymentRepository.Get(paymentId);
            return Ok(new PaymentDto(payment));
        }

        [HttpPut("payments/{paymentId:long}")]
        public async Task<IActionResult> Update(long accountId, long paymentId, [FromBody] NewPaymentDto newPaymentDto)
        {
            var payment = await paymentRepository.Get(paymentId);

            payment.Date = newPaymentDto.Date.ToDateTime();
            payment.Type = newPaymentDto.Type;
            payment.CategoryId = newPaymentDto.CategoryId;
            payment.Amount = newPaymentDto.Amount;
            payment.UserId = newPaymentDto.UserId;
            payment.Description = newPaymentDto.Description;

            await paymentRepository.Save();

            return Ok(new PaymentDto(payment));
        }

        [HttpPost("payments")]
        public async Task<IActionResult> Create(long accountId, [FromBody] NewPaymentDto payment)
        {
            var savedPayment = paymentRepository.Add(new Payment()
            {
                AccountId = accountId,
                CategoryId = payment.CategoryId,
                Amount = payment.Amount,
                Date = payment.Date.ToDateTime(),
                Description = payment.Description,
                Type = payment.Type,
                UserId = payment.UserId
            });

            await paymentRepository.Save();
            return Ok(savedPayment);
        }

        public class NewPaymentDto
        {
            public DateOnly Date { get; set; }
            public string Description { get; set; } = default!;
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
                this.Date = payment.Date.ToDateOnly();
                this.Description = payment.Description;
                this.CategoryId = payment.CategoryId;
                this.UserId = payment.UserId;
                this.Amount = payment.Amount;
                this.Type = payment.Type;
            }

            public long Id { get; set; }
            public long AccountId { get; set; }
            public DateOnly Date { get; set; }
            public string Description { get; set; }
            public decimal Amount { get; set; }
            public PaymentType Type { get; set; }
            public long CategoryId { get; set; }
            public Guid UserId { get; set; }
        }

        public class Filter
        {
            public DateOnly? From { get; set; }
            public DateOnly? To { get; set; }
            public PaymentType Type { get; set; }
        }
    }
}