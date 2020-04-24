using Badzeet.Domain.Budget;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult List()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Transform(long id, long accountId)
        {
            await service.Transform(id, accountId);
            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}