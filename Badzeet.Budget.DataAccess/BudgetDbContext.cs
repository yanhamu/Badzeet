using Badzeet.Budget.DataAccess.Maps;
using Microsoft.EntityFrameworkCore;

namespace Badzeet.Budget.DataAccess;

public class BudgetDbContext : DbContext
{
    public BudgetDbContext(DbContextOptions<BudgetDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("budget");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountMap).Assembly);
    }
}