using Badzeet.Domain.Book.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Features.Categories
{
    [Authorize]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index(long bookId)
        {
            var categories = await categoryRepository.GetCategories(bookId);
            var c = categories.Select(x => new CategoryViewModel() { Id = x.Id, Name = x.Name, Order = x.Order }).ToList();
            var model = new CategoryListViewModel() { Categories = c };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(long id)
        {
            var category = await categoryRepository.Get(id);
            var model = new CategoryViewModel() { Id = category.Id, Name = category.Name, Order = category.Order };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryViewModel model, long bookId)
        {
            var category = await categoryRepository.Get(model.Id);
            category.Name = model.Name;
            category.Order = model.Order;
            await categoryRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult New(long bookId)
        {
            return View(new CategoryViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> New(long bookId, CategoryViewModel model)
        {
            await categoryRepository.Create(bookId, model.Name, model.Order);
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
            public List<CategoryViewModel> Categories { get; set; }
        }

        public class CategoryViewModel
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public int Order { get; set; }
        }
    }
}