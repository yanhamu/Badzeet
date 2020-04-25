using Badzeet.Budget.DataAccess;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Budget
{
    public class CategoryBudgetRepository : ICategoryBudgetRepository
    {
        private readonly BudgetDbContext dbContext;

        public CategoryBudgetRepository(BudgetDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddBudget(CategoryBudget categoryBudget)
        {
            dbContext
                .Set<CategoryBudget>()
                .Add(categoryBudget);
        }

        public Task<List<CategoryBudget>> GetBudgets(long accountId, int budgetId)
        {
            return dbContext
                .Set<CategoryBudget>()
                .Where(x => x.AccountId == accountId)
                .Where(x => x.Id == budgetId)
                .ToListAsync();
        }

        public Task<bool> HasBudget(long accountId, int budgetId)
        {
            return dbContext.Set<CategoryBudget>().AnyAsync(x => x.AccountId == accountId && x.Id == budgetId);
        }

        public Task Save()
        {
            return dbContext.SaveChangesAsync();
        }
    }
}