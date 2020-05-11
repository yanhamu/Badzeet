using Badzeet.Scheduler.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Badzeet.Scheduler.DataAccess
{
    public class SchedulerDbContext : DbContext
    {
        public SchedulerDbContext(DbContextOptions<SchedulerDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("scheduler");

            var log = modelBuilder.Entity<Log>();
            log.ToTable("logs");
            log.HasKey(x => x.Id);
            log.Property(x => x.StartedAt).HasColumnName("started_at");
            log.Property(x => x.FinishedAt).HasColumnName("finished_at");
            log.Property(x => x.RowsProcessed).HasColumnName("processed_rows");

            var payment = modelBuilder.Entity<Payment>();
            payment.ToTable("payments");
            payment.HasKey(x => x.Id);
            payment.Property(x => x.AccountId).HasColumnName("account_id");
            payment.Property(x => x.Amount).HasColumnName("amount");
            payment.Property(x => x.Description).HasColumnName("description");
            payment.Property(x => x.CategoryId).HasColumnName("category_id");
            payment.Property(x => x.OwnerId).HasColumnName("owner_id");
            payment.Property(x => x.UpdatedAt).HasColumnName("updated_at");
            payment.Property(x => x.ScheduledAt).HasColumnName("scheduled_at");
            payment.Property(x => x.SchedulingType).HasColumnName("scheduling_type");
            payment.Property(x => x.Metadata).HasColumnName("scheduling_metadata");
        }
    }
}