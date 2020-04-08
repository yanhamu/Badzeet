using System;

namespace Badzeet.Domain.Book.Model
{
    public class UserAccount
    {
        public Guid UserId { get; set; }
        public long AccountId { get; set; }
        public User User { get; set; }
        public Account Account { get; set; }
    }
}
