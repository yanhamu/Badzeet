using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Badzeet.Budget.DataAccess.Maps;

internal class BudgetMap : IEntityTypeConfiguration<Domain.Model.Budget>
{
    public void Configure(EntityTypeBuilder<Domain.Model.Budget> builder)
    {
        builder.ToTable("budgets");
        builder.HasKey(x => new { x.Id });
        builder.Property(x => x.BudgetId).HasColumnName("budget_id");
        builder.Property(x => x.AccountId).HasColumnName("account_id");
        builder.Property(x => x.DateFrom).HasColumnName("date_from");
        builder.Property(x => x.DateTo).HasColumnName("date_to");
        builder.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId);
    }
}