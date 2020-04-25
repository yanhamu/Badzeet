using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Budget
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BudgetDbContext dbContext;

        public CategoryRepository(BudgetDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task Create(long bookId, string name, int order)
        {
            _ = dbContext.Set<Category>().Add(new Category() { Name = name, AccountId = bookId, Order = order });
            return dbContext.SaveChangesAsync();
        }

        public async Task<Category> Remove(long categoryId)
        {
            var category = await Get(categoryId);
            dbContext.Remove(category);
            await dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category> Get(long categoryId)
        {
            var category = await dbContext.Set<Category>().FindAsync(categoryId);
            return category;
        }

        public Task<List<Category>> GetCategories(long bookId)
        {
            return dbContext.Set<Category>()
                .Where(x => x.AccountId == bookId)
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Name)
                .ToListAsync();
        }

        public Task Save()
        {
            return dbContext.SaveChangesAsync();
        }
    }
}
