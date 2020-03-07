using Badzeet.Domain.Book.Model;
using System;

namespace Badzeet.Web.Features.Book
{
    public class TransactionModel
    {
        public TransactionModel() { }

        public TransactionModel(long id, DateTime date, string description, decimal amount, long? categoryId)
        {
            Id = id;
            Date = date;
            Description = description;
            Amount = amount;
            this.CategoryId = categoryId;
        }

        public TransactionModel(Transaction transaction)
        {
            this.Id = transaction.Id;
            this.Date = transaction.Date;
            this.Description = transaction.Description;
            this.Amount = transaction.Amount;
            this.CategoryId = transaction.CategoryId;
        }

        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public long? CategoryId { get; set; }
    }
}