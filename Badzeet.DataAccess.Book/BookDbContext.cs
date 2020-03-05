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
            transaction.Property(x => x.AccountId).HasColumnName("account_id");
        }


    }
}