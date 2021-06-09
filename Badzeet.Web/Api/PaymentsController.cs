using Badzeet.Budget.Domain;
using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Badzeet.Web.Api
{
    [Route("api/accounts/{accountId:long}/payments")]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentRepository paymentRepository;

        public PaymentsController(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public async Task<object> List(long accountId, Filter filter)
        {
            var payments = await paymentRepository.GetPayments(new PaymentsFilter(accountId, interval: filter.Interval));
            return Ok(payments);
        }
    }

    public class Filter
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public DateInterval Interval { get => new DateInterval(From, To); }
    }
}
