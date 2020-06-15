﻿using Badzeet.Budget.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Badzeet.Budget.DataAccess
{
    public class BudgetDbContext : DbContext
    {
        public BudgetDbContext(DbContextOptions<BudgetDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("budget");

            var payment = modelBuilder.Entity<Payment>();
            payment.ToTable("payments");
            payment.HasKey(x => x.Id);
            payment.Property(x => x.AccountId).HasColumnName("account_id");
            payment.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId);
            payment.Property(x => x.CategoryId).HasColumnName("category_id");
            payment.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId);
            payment.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            payment.Property(x => x.UserId).HasColumnName("owner_id");
            payment.Property(x => x.Type).HasColumnName("type");

            var account = modelBuilder.Entity<Account>();
            account.ToTable("accounts");
            account.HasKey(x => x.Id);
            account.Property(x => x.FirstDayOfTheBudget).HasColumnName("first_day_of_budget");
            account.Property(x => x.CreatedAt).HasColumnName("created_at");

            var category = modelBuilder.Entity<Category>();
            category.ToTable("categories");
            category.HasKey(x => x.Id);
            category.Property(x => x.AccountId).HasColumnName("account_id");

            var userAccount = modelBuilder.Entity<UserAccount>();
            userAccount.ToTable("user_accounts");
            userAccount.HasKey(x => new { x.UserId, x.AccountId });
            userAccount.Property(x => x.UserId).HasColumnName("user_id");
            userAccount.Property(x => x.AccountId).HasColumnName("account_id");
            userAccount.Property(x => x.Balance).HasColumnName("balance").IsRequired(false);
            userAccount.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
            userAccount.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId);

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

            var categoryBudget = modelBuilder.Entity<CategoryBudget>();
            categoryBudget.ToTable("category_budget");
            categoryBudget.HasKey(x => new { x.AccountId, x.Id, x.CategoryId });
            categoryBudget.Property(x => x.AccountId).HasColumnName("account_id");
            categoryBudget.Property(x => x.CategoryId).HasColumnName("category_id");
            categoryBudget.Property(x => x.Amount).HasColumnName("amount");
            categoryBudget.HasOne(x => x.Account).WithMany().HasForeignKey(x => x.AccountId);
            categoryBudget.HasOne(x => x.Category).WithMany().HasForeignKey(x => x.CategoryId);
        }
    }
}