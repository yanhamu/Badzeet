using Badzeet.Budget.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain.Interfaces
{
    public interface ICategoryBudgetRepository
    {
        Task<List<CategoryBudget>> GetBudgets(long accountId, int budgetId);
        void AddBudget(CategoryBudget categoryBudget);
        Task<bool> HasBudget(long accountId, int budgetId);
        Task Save();
    }
}
