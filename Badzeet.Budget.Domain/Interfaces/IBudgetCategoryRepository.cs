using Badzeet.Budget.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain.Interfaces
{
    public interface IBudgetCategoryRepository
    {
        Task<List<BudgetCategory>> GetBudgetCategories(long budgetId);
        void AddBudget(BudgetCategory categoryBudget);
        Task Save();
    }
}
