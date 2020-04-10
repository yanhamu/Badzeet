using Badzeet.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Badzeet.Web.Features.Home
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return User.Identity.IsAuthenticated
                ? (IActionResult)RedirectToAction("Index", "Dashboard")
                : View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}