using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Badzeet.Web.Features.Categories;

[Authorize]
public class CategoryController : Controller
{
    private readonly ICategoryRepository categoryRepository;

    public CategoryController(ICategoryRepository categoryRepository)
    {
        this.categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> Index(long accountId)
    {
        var categories = await categoryRepository.GetCategories(accountId);
        var c = categories.Select(x => new CategoryViewModel { Id = x.Id, Name = x.Name, Order = x.Order, InSummary = x.DisplayInSummary }).ToList();
        var model = new CategoryListViewModel { Categories = c };
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(long id)
    {
        var category = await categoryRepository.Get(id);
        var model = new CategoryViewModel { Id = category.Id, Name = category.Name, Order = category.Order, InSummary = category.DisplayInSummary };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(long accountId, CategoryViewModel model)
    {
        var category = await categoryRepository.Get(model.Id);
        category.Name = model.Name;
        category.Order = model.Order;
        category.DisplayInSummary = model.InSummary;
        await categoryRepository.Save();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult New()
    {
        return View(new CategoryViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> New(long accountId, CategoryViewModel model)
    {
        await categoryRepository.Create(accountId, model.Name, model.Order);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Remove(long Id)
    {
        await categoryRepository.Remove(Id);
        return RedirectToAction(nameof(Index));
    }

    public class CategoryListViewModel
    {
        public List<CategoryViewModel> Categories { get; set; } = new();
    }

    public class CategoryViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; } = default!;
        public int Order { get; set; }
        public bool InSummary { get; set; }
    }
}