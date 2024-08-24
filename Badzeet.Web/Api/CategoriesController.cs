using System.Linq;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.AspNetCore.Mvc;

namespace Badzeet.Web.Api;

[Route("api/accounts/{accountId:long}/categories")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryRepository categoryRepository;

    public CategoriesController(ICategoryRepository categoryRepository)
    {
        this.categoryRepository = categoryRepository;
    }

    [HttpGet]
    public async Task<IActionResult> List(long accountId)
    {
        var categories = await categoryRepository.GetCategories(accountId);
        return Ok(categories.Select(x => new CategoryDto(x)));
    }

    public class CategoryDto
    {
        public CategoryDto(Category category)
        {
            Id = category.Id;
            AccountId = category.AccountId;
            Name = category.Name;
            Order = category.Order;
        }

        public long Id { get; set; }
        public long AccountId { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
    }
}