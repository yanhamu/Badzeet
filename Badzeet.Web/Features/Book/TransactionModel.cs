using Badzeet.Domain.Book.Model;
using System;

namespace Badzeet.Web.Features.Book
{
    public class TransactionModel
    {
        public TransactionModel() { }

        public TransactionModel(long id, DateTime date, string description, decimal amount, long categoryId, Guid userId)
        {
            Id = id;
            Date = date;
            Description = description;
            Amount = amount;
            this.CategoryId = categoryId;
            this.UserId = userId;
        }

        public TransactionModel(Transaction transaction)
        {
            this.Id = transaction.Id;
            this.Date = transaction.Date;
            this.Description = transaction.Description;
            this.Amount = transaction.Amount;
            this.CategoryId = transaction.CategoryId;
            this.UserId = transaction.UserId;
        }

        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public long CategoryId { get; set; }
        public Guid UserId { get; set; }
    }
}