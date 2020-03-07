using Badzeet.Domain.Book;
using Microsoft.EntityFrameworkCore;

namespace Badzeet.DataAccess.Book
{
    public class BookDbContext : DbContext
    {
        public BookDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("book");

            var transaction = modelBuilder.Entity<Transaction>();
            transaction.ToTable("transactions");
            transaction.HasKey(x => x.Id);
            transaction.Property(x => x.BookId).HasColumnName("book_id");
            transaction.HasOne(x => x.Book).WithMany().HasForeignKey(x => x.BookId);

            var book = modelBuilder.Entity<Badzeet.Domain.Book.Model.Book>();
            book.ToTable("books");
            book.HasKey(x => x.Id);
            book.Property(x => x.FirstDayOfTheBudget).HasColumnName("first_day_of_budget");
        }
    }
}