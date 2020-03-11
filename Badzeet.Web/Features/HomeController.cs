using Badzeet.Domain.Book;
using Badzeet.Web.Configuration;
using Badzeet.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Badzeet.Web.Features
{
    public class HomeController : Controller
    {
        private readonly BudgetService budgetService;

        public HomeController(BudgetService budgetService)
        {
            this.budgetService = budgetService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var interval = await budgetService.GetLatestBudget(HttpContext.GetBookId());
                var budget = await budgetService.GetBudget(HttpContext.GetBookId(), interval);
                
                return View(new DashboardViewModel(interval, budget));
            }
            else
            {
                return View();
            }
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