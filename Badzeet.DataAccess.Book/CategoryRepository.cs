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

        public Task<List<Category>> GetCategories(long bookId)
        {
            return dbContext.Set<Category>().Where(x => x.BookId == bookId).ToListAsync();
        }
    }
}
