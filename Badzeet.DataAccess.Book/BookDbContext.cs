using Badzeet.Domain.Book.Model;
using Microsoft.EntityFrameworkCore;

namespace Badzeet.DataAccess.Book
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("budget");

            var transaction = modelBuilder.Entity<Transaction>();
            transaction.ToTable("transactions");
            transaction.HasKey(x => x.Id);
            transaction.Property(x => x.AccountId).HasColumnName("account_id");
            transaction.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId);
            transaction.Property(x => x.CategoryId).HasColumnName("category_id");
            transaction.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId);
            transaction.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            transaction.Property(x => x.UserId).HasColumnName("owner_id");

            var book = modelBuilder.Entity<Badzeet.Domain.Book.Model.Account>();
            book.ToTable("accounts");
            book.HasKey(x => x.Id);
            book.Property(x => x.FirstDayOfTheBudget).HasColumnName("first_day_of_budget");
            book.Property(x => x.CreatedAt).HasColumnName("created_at");

            var category = modelBuilder.Entity<Category>();
            category.ToTable("categories");
            category.HasKey(x => x.Id);
            category.Property(x => x.AccountId).HasColumnName("account_id");

            var userBook = modelBuilder.Entity<UserAccount>();
            userBook.ToTable("user_accounts");
            userBook.HasKey(x => new { x.UserId, x.AccountId });
            userBook.Property(x => x.UserId).HasColumnName("user_id");
            userBook.Property(x => x.AccountId).HasColumnName("account_id");

            userBook.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);

            var user = modelBuilder.Entity<User>();
            user.ToTable("users");
            user.HasKey(x => x.Id);
            user.Property(x => x.Id).HasColumnName("id");
            user.Property(x => x.Nickname).HasColumnName("nickname");

            var invitation = modelBuilder.Entity<Invitation>();
            invitation.ToTable("invitations");
            invitation.HasKey(x => x.Id);
            invitation.Property(x => x.Id).HasColumnName("id");
            invitation.Property(x => x.AccountId).HasColumnName("account_id");
            invitation.Property(x => x.OwnerId).HasColumnName("owner_id");
            invitation.Property(x => x.UsedAt).HasColumnName("used_at");
            invitation.Property(x => x.CreatedAt).HasColumnName("created_at");

            invitation.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId);
            invitation.HasOne(x => x.User).WithMany().HasForeignKey(x => x.OwnerId);
        }
    }
}