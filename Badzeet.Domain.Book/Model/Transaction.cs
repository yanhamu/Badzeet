using System;

namespace Badzeet.Domain.Book.Model
{
    public class Transaction
    {
        public Transaction()
        {

        }

        public Transaction(long id, DateTime date, string description, decimal amount)
        {
            Id = id;
            Date = date;
            Description = description;
            Amount = amount;
        }

        public long Id { get; set; }
        public long BookId { get; set; }
        public Book Book { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public long? CategoryId { get; set; }
        public Category Category { get; set; }
    }
}