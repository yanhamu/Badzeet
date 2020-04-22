using Microsoft.AspNetCore.Mvc;

namespace Badzeet.Web.Features.ScheduledPayments
{
    public class ScheduledPaymentsController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
