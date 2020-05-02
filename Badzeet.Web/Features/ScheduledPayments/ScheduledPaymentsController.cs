using Badzeet.Budget.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.ScheduledPayments
{
    [Authorize]
    public class ScheduledPaymentsController : Controller
    {
        private readonly ScheduledPaymentsService service;

        public ScheduledPaymentsController(ScheduledPaymentsService scheduledPaymentService)
        {
            this.service = scheduledPaymentService;
        }

        public async Task<IActionResult> List(long accountId, Guid userId)
        {
            var payments = await service.GetPayments(userId, accountId);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Transform(long id, long accountId)
        {
            await service.Transform(id);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}