using Badzeet.Domain.Book.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Domain.Book.Interfaces
{
    public interface ICategoryBudgetRepository
    {
        Task<List<CategoryBudget>> GetBudgets(long accountId, int budgetId);
        void AddBudget(CategoryBudget categoryBudget);
        Task<bool> HasBudget(long accountId, int budgetId);
        Task Save();
    }
}
