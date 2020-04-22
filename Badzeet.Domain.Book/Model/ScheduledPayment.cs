using Badzeet.Domain.Book.Model;
using System;

namespace Badzeet.Domain.Budget.Model
{
    public class ScheduledPayment
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public long AccountId { get; set; }
        public Account Account { get; set; }
        public long CategoryId { get; set; }
        public Category Category { get; set; }
        public Guid OwnerId { get; set; }
        public User User { get; set; }
        public DateTime Date { get; set; }
    }
}