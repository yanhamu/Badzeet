﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Interfaces;
using Badzeet.Budget.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Badzeet.Budget.DataAccess;

public class BudgetCategoryRepository : IBudgetCategoryRepository
{
    private readonly BudgetDbContext dbContext;

    public BudgetCategoryRepository(BudgetDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void AddBudget(BudgetCategory categoryBudget)
    {
        dbContext
            .Set<BudgetCategory>()
            .Add(categoryBudget);
    }

    public Task<List<BudgetCategory>> GetBudgetCategories(int budgetId, long accountId)
    {
        return dbContext
            .Set<BudgetCategory>()
            .Where(x => x.BudgetId == budgetId)
            .Where(x => x.AccountId == accountId)
            .ToListAsync();
    }

    public Task Save()
    {
        return dbContext.SaveChangesAsync();
    }
}