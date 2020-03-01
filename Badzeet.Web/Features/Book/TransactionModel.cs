using System;

namespace Badzeet.Web.Features.Book
{
    public class TransactionModel
    {
        public TransactionModel() { }

        public TransactionModel(long id, DateTime date, string description, decimal amount)
        {
            Id = id;
            Date = date;
            Description = description;
            Amount = amount;
        }

        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}