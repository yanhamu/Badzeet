using Badzeet.Budget.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategories(Guid accountId);
        Task Create(Guid id, Guid accountId, string name, int order);
        Task<Category?> Remove(Guid categoryId);
        Task<Category?> Get(Guid categoryId);
        Task Save();
    }
}
