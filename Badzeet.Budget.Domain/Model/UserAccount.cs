using System;

namespace Badzeet.Budget.Domain.Model;

public class UserAccount
{
    public Guid UserId { get; set; }
    public long AccountId { get; set; }
    public User User { get; set; } = default!;
    public Account Account { get; set; } = default!;
}