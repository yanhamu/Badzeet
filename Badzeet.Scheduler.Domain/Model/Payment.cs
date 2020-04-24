using System;

namespace Badzeet.Scheduler.Domain.Model
{
    public class Payment
    {
        public long Id { get; set; }
        public long AccountId { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ScheduledAt { get; set; }
        public SchedulingType SchedulingType { get; set; }
        public string Metadata { get; set; }
    }
}