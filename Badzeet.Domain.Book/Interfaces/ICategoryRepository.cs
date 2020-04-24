using Badzeet.Domain.Budget.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Domain.Budget.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories(long bookId);
        Task Create(long bookId, string name, int order);
        Task<Category> Remove(long categoryId);
        Task<Category> Get(long categoryId);
        Task Save();
    }
}
