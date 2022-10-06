using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Api.Filters
{
    public class PaymentIdFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var paymentId = (long)context.ActionArguments["paymentId"];
            var userAccountRepository = (IUserAccountRepository)context.HttpContext.RequestServices.GetService(typeof(IUserAccountRepository));
            var paymentRepository = (IPaymentRepository)context.HttpContext.RequestServices.GetService(typeof(IPaymentRepository));

            var userId = new Guid(context.HttpContext.User.Claims.Single(x => x.Type == "sub").Value);
            var accounts = await userAccountRepository.GetUserAccounts(userId);
            var payment = await paymentRepository.Get(paymentId);

            if (payment == null)
            {
                context.Result = new StatusCodeResult(404);
            }
            else if (accounts.Any(a => a.AccountId == payment.AccountId))
            {
                await next();
            }
            else
            {
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
