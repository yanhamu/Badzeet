using Badzeet.Budget.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Badzeet.Budget.DataAccess.Maps
{
    internal class InvitationMap : IEntityTypeConfiguration<Invitation>
    {
        public void Configure(EntityTypeBuilder<Invitation> builder)
        {
            builder.ToTable("invitations");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("id");
            builder.Property(x => x.AccountId).HasColumnName("account_id");
            builder.Property(x => x.OwnerId).HasColumnName("owner_id");
            builder.Property(x => x.UsedAt).HasColumnName("used_at");
            builder.Property(x => x.CreatedAt).HasColumnName("created_at");
            builder.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.OwnerId);
        }
    }
}