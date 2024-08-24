using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Badzeet.Budget.DataAccess;

public class CategoryRepository : ICategoryRepository
{
    private readonly BudgetDbContext dbContext;

    public CategoryRepository(BudgetDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public Task Create(long accountId, string name, int order)
    {
        _ = dbContext.Set<Category>().Add(new Category { Name = name, AccountId = accountId, Order = order });
        return dbContext.SaveChangesAsync();
    }

    public async Task<Category?> Remove(long categoryId)
    {
        var category = await Get(categoryId);
        if (category == null)
            return null;

        dbContext.Remove(category);
        await dbContext.SaveChangesAsync();
        return category;
    }

    public async Task<Category?> Get(long categoryId)
    {
        return await dbContext.Set<Category>().FindAsync(categoryId);
    }

    public Task<List<Category>> GetCategories(long accountId)
    {
        return dbContext.Set<Category>()
            .Where(x => x.AccountId == accountId)
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Name)
            .ToListAsync();
    }

    public Task Save()
    {
        return dbContext.SaveChangesAsync();
    }
}