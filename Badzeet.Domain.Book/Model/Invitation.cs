using System;

namespace Badzeet.Domain.Book.Model
{
    public class Invitation
    {
        public Guid Id { get; set; }
        public Guid OwnerId { get; set; }
        public User User { get; set; }
        public long BookId { get; set; }
        public Book Book { get; set; }
        public DateTime? UsedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
