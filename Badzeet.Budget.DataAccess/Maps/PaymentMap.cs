using Badzeet.Budget.Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Badzeet.Budget.DataAccess.Maps
{
    internal class PaymentMap : IEntityTypeConfiguration<Payment>
    {
        public void Configure(EntityTypeBuilder<Payment> builder)
        {
            builder.ToTable("payments");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AccountId).HasColumnName("account_id");
            builder.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId);
            builder.Property(x => x.CategoryId).HasColumnName("category_id");
            builder.HasOne(x=>x.Category).WithMany().HasForeignKey(x => x.CategoryId);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            builder.Property(x => x.UserId).HasColumnName("owner_id");
            builder.Property(x => x.Type).HasColumnName("type");
        }
    }
}
