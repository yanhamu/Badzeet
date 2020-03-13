using Badzeet.Domain.Book.Model;
using Microsoft.EntityFrameworkCore;

namespace Badzeet.DataAccess.Book
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("book");

            var transaction = modelBuilder.Entity<Transaction>();
            transaction.ToTable("transactions");
            transaction.HasKey(x => x.Id);
            transaction.Property(x => x.BookId).HasColumnName("book_id");
            transaction.HasOne(x => x.Book).WithMany().HasForeignKey(x => x.BookId);
            transaction.Property(x => x.CategoryId).HasColumnName("category_id");
            transaction.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId);
            transaction.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            transaction.Property(x => x.UserId).HasColumnName("owner_id");

            var book = modelBuilder.Entity<Badzeet.Domain.Book.Model.Book>();
            book.ToTable("books");
            book.HasKey(x => x.Id);
            book.Property(x => x.FirstDayOfTheBudget).HasColumnName("first_day_of_budget");

            var category = modelBuilder.Entity<Category>();
            category.ToTable("categories");
            category.HasKey(x => x.Id);
            category.Property(x => x.BookId).HasColumnName("book_id");

            var userBook = modelBuilder.Entity<UserBook>();
            userBook.ToTable("user_books");
            userBook.HasKey(x => new { x.UserId, x.BookId });
            userBook.Property(x => x.UserId).HasColumnName("user_id");
            userBook.Property(x => x.BookId).HasColumnName("book_id");
            userBook.Property(x => x.Nickname).HasColumnName("nickname");

            userBook.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);

            var user = modelBuilder.Entity<User>();
            user.ToTable("users");
            user.HasKey(x => x.Id);
            user.Property(x => x.Id).HasColumnName("id");
        }
    }
}