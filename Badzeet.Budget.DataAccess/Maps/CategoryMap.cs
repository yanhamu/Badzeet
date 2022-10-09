using Badzeet.Budget.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Badzeet.Budget.DataAccess.Maps
{
    internal class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AccountId).HasColumnName("account_id");

            builder.Property(x => x.DisplayInSummary).HasColumnName("in_summary");
        }
    }
}
