using Microsoft.AspNetCore.Mvc;

namespace Badzeet.Web.Features.RecurringPayments
{
    public class RecurringPaymentsController : Controller
    {
        public IActionResult List()
        {
            return View();
        }
    }
}
