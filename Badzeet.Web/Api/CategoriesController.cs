using Badzeet.Budget.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Web.Api
{
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
            public long Id { get; set; }
            public long AccountId { get; set; }
            public string Name { get; set; }
            public int Order { get; set; }

            public CategoryDto(Budget.Domain.Model.Category category)
            {
                this.Id = category.Id;
                this.AccountId = category.AccountId;
                this.Name = category.Name;
                this.Order = category.Order;
            }
        }
    }
}
