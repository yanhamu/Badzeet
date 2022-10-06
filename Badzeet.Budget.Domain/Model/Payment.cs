using System;

namespace Badzeet.Budget.Domain.Model
{
    public class Payment
    {
        public Payment() { }

        public Payment(DateTime date, string description, decimal amount, long categoryId, Guid userId, PaymentType paymentType, long accountId)
        {
            AccountId = accountId;
            CategoryId = categoryId;
            UserId = userId;
            Date = date;
            Description = description;
            Amount = amount;
            Type = paymentType;
        }

        public Payment(long id, DateTime date, string description, decimal amount, long categoryId, Guid userId, PaymentType paymentType, long accountId) : this(date, description, amount, categoryId, userId, paymentType, accountId)
        {
            Id = id;
        }

        public long Id { get; set; }
        public long AccountId { get; set; }
        public Account Account { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public PaymentType Type { get; set; }
    }

    public enum PaymentType : byte
    {
        Normal = 1,
        Scheduled = 2,
        Pending = 3
    }
}