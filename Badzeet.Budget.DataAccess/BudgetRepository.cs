using Badzeet.Budget.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Badzeet.Budget.DataAccess
{
    public class BudgetRepository : IBudgetRepository
    {
        private readonly BudgetDbContext context;

        public BudgetRepository(BudgetDbContext dbContext)
        {
            this.context = dbContext;
        }

        public Domain.Model.Budget Create(Domain.Model.Budget budget)
        {
            var saved = context.Set<Domain.Model.Budget>().Add(budget);
            return saved.Entity;
        }

        public async ValueTask<Domain.Model.Budget> Get(int budgetId, long accountId)
        {
            return await context.Set<Domain.Model.Budget>().FindAsync(budgetId, accountId);
        }

        public async Task<List<Domain.Model.Budget>> List(long accountId, Filter filter)
        {
            var query = context.Set<Domain.Model.Budget>().AsQueryable();
            if (filter?.From != null)
                query = query.Where(x => x.Date >= filter.From.Value);

            if (filter?.To != null)
                query = query.Where(x => x.Date <= filter.To.Value);

            return await query.ToListAsync();
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
