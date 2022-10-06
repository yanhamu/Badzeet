using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Badzeet.Web.Api
{
    [Route("api/transactions")]
    public class TransactionsController : ControllerBase
    {
        private readonly IPaymentRepository paymentRepository;

        public TransactionsController(IPaymentRepository paymentRepository)
        {
            this.paymentRepository = paymentRepository;
        }

        public async Task<object> List(long accountId, Filter filter)
        {
            var payments = await paymentRepository.GetPayments(new PaymentsFilter(accountId, filter.From, filter.To));
            return payments;
        }
    }
}
