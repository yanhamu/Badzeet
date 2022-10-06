using Badzeet.Budget.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain.Interfaces
{
    public interface IBudgetCategoryRepository
    {
        Task<List<BudgetCategory>> GetBudgetCategories(int budgetId, long accountId);
        void AddBudget(BudgetCategory categoryBudget);
        Task Save();
    }
}
