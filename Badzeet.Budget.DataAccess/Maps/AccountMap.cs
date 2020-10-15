using Badzeet.Budget.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Badzeet.Budget.DataAccess.Maps
{
    internal class AccountMap : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("accounts");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.FirstDayOfTheBudget).HasColumnName("first_day_of_budget");
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
        }
    }
}
