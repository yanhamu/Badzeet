using Badzeet.Domain.Book.Interfaces;
using Badzeet.Domain.Book.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.DataAccess.Book
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly BookDbContext dbContext;

        public CategoryRepository(BookDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task Create(long bookId, string name)
        {
            _ = dbContext.Set<Category>().Add(new Category() { Name = name, BookId = bookId });
            return dbContext.SaveChangesAsync();
        }

        public async Task<Category> Get(long categoryId)
        {
            var category = await dbContext.Set<Category>().FindAsync(categoryId);
            return category;
        }

        public Task<List<Category>> GetCategories(long bookId)
        {
            return dbContext.Set<Category>()
                .Where(x => x.BookId == bookId)
                .ToListAsync();
        }

        public Task Save()
        {
            return dbContext.SaveChangesAsync();
        }
    }
}
