using Badzeet.Budget.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Badzeet.Budget.DataAccess.Maps;

internal class UserAccountMap : IEntityTypeConfiguration<UserAccount>
{
    public void Configure(EntityTypeBuilder<UserAccount> builder)
    {
        builder.ToTable("user_accounts");
        builder.HasKey(x => new { x.UserId, x.AccountId });
        builder.Property(x => x.UserId).HasColumnName("user_id");
        builder.Property(x => x.AccountId).HasColumnName("account_id");
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
        builder.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId);
    }
}