using System;

namespace Badzeet.Scheduler.Domain.Model
{
    public class Payment
    {
        public Guid Id { get; set; }
        public long AccountId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = default!;
        public Guid CategoryId { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime ScheduledAt { get; set; }
        public SchedulingType SchedulingType { get; set; }
        public string Metadata { get; set; } = default!;
    }
}