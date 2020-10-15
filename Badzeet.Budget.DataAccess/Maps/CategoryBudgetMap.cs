using Badzeet.Budget.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Badzeet.Budget.DataAccess.Maps
{
    internal class CategoryBudgetMap : IEntityTypeConfiguration<CategoryBudget>
    {
        public void Configure(EntityTypeBuilder<CategoryBudget> builder)
        {
            builder.ToTable("category_budget");
            builder.HasKey(x => new { x.AccountId, x.Id, x.CategoryId });
            builder.Property(x => x.AccountId).HasColumnName("account_id");
            builder.Property(x => x.CategoryId).HasColumnName("category_id");
            builder.Property(x => x.Amount).HasColumnName("amount");
            builder.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId);
            builder.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId);

        }
    }
}
