using System.Collections.Generic;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Model;

namespace Badzeet.Budget.Domain.Interfaces;

public interface IBudgetCategoryRepository
{
    Task<List<BudgetCategory>> GetBudgetCategories(int budgetId, long accountId);
    void AddBudget(BudgetCategory categoryBudget);
    Task Save();
}