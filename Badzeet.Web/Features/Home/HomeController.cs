using System.Diagnostics;
using System.Threading.Tasks;
using Badzeet.Web.Configuration;
using Badzeet.Web.Features.Common;
using Badzeet.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Badzeet.Web.Features.Home;

public class HomeController : Controller
{
    private readonly BudgetNavigationService budgetNavigationService;
    private readonly IHttpContextAccessor httpContextAccessor;

    public HomeController(BudgetNavigationService budgetNavigationService, IHttpContextAccessor httpContextAccessor)
    {
        this.budgetNavigationService = budgetNavigationService;
        this.httpContextAccessor = httpContextAccessor;
    }

    public async Task<IActionResult> Index()
    {
        if (User.Identity!.IsAuthenticated == false)
            return View();

        var accountId = httpContextAccessor.HttpContext!.GetAccountId();
        var nav = await budgetNavigationService.Get(accountId);

        return RedirectToAction("Index", "Budget", new { budgetId = nav.Current.BudgetId });
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