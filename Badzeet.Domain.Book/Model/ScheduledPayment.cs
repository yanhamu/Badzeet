using System;

namespace Badzeet.Domain.Budget.Model
{
    public class ScheduledPayment
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public long CategoryId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime Date { get; set; }
    }
}