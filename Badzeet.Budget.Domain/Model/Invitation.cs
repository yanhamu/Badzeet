using System;

namespace Badzeet.Budget.Domain.Model
{
    public class Invitation
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public User User { get; set; }
        public long AccountId { get; set; }
        public Account Account { get; set; }
        public DateTime? UsedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
