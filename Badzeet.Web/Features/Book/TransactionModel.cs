using System;

namespace Badzeet.Web.Features.Book
{
    public class TransactionModel
    {
        public TransactionModel(long id, DateTime date, string description, decimal amount)
        {
            Id = id;
            Date = date;
            Description = description;
            Amount = amount;
        }

        public long Id { get; }
        public DateTime Date { get; }
        public string Description { get; }
        public decimal Amount { get; }
    }
}