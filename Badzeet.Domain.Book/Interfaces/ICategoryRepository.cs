using Badzeet.Domain.Book.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Domain.Book.Interfaces
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
