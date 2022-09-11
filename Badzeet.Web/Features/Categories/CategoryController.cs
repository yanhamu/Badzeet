using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        public async Task<IActionResult> Index(Guid accountId)
        {
            var categories = await categoryRepository.GetCategories(accountId);
            var c = categories.Select(x => new CategoryViewModel() { Id = x.Id, Name = x.Name, Order = x.Order }).ToList();
            var model = new CategoryListViewModel() { Categories = c };
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await categoryRepository.Get(id);
            var model = new CategoryViewModel() { Id = category.Id, Name = category.Name, Order = category.Order };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid accountId, CategoryViewModel model)
        {
            var category = await categoryRepository.Get(model.Id);
            category.Name = model.Name;
            category.Order = model.Order;
            await categoryRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult New()
        {
            return View(new CategoryViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> New(Guid accountId, CategoryViewModel model)
        {
            await categoryRepository.Create(Guid.NewGuid(), accountId, model.Name, model.Order);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Remove(Guid Id)
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
            public Guid Id { get; set; }
            public string Name { get; set; }
            public int Order { get; set; }
        }
    }
}