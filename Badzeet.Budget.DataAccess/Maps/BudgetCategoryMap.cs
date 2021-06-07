using Badzeet.Budget.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Badzeet.Budget.DataAccess.Maps
{
    internal class BudgetCategoryMap : IEntityTypeConfiguration<BudgetCategory>
    {
        public void Configure(EntityTypeBuilder<BudgetCategory> builder)
        {
            builder.ToTable("budget_categories");
            builder.HasKey(x => new { x.BudgetId, x.CategoryId });

            builder.Property(x => x.BudgetId).HasColumnName("budget_id");
            builder.Property(x => x.CategoryId).HasColumnName("category_id");
            builder.Property(x => x.Amount).HasColumnName("amount");

            builder.HasOne(x => x.Budget).WithMany().HasForeignKey(x => x.BudgetId);
            builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId);
        }
    }
}
