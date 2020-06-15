using System;

namespace Badzeet.Budget.Domain.Model
{
    public class UserAccount
    {
        public Guid UserId { get; set; }
        public long AccountId { get; set; }
        public User User { get; set; }
        public Account Account { get; set; }
        public decimal? Balance { get; set; }
    }
}
