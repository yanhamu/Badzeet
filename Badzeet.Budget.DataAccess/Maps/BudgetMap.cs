using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Badzeet.Budget.DataAccess.Maps
{
    internal class BudgetMap : IEntityTypeConfiguration<Domain.Model.Budget>
    {
        public void Configure(EntityTypeBuilder<Domain.Model.Budget> builder)
        {
            builder.ToTable("budgets");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.AccountId).HasColumnName("account_id");
            builder.Property(x => x.Date).HasColumnName("date");
            builder.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId);
        }
    }
}
