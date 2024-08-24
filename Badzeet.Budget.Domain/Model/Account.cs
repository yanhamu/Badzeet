using System;

namespace Badzeet.Budget.Domain.Model;

public class Account
{
    public long Id { get; set; }
    public byte FirstDayOfTheBudget { get; set; }
    public DateTime CreatedAt { get; set; }
}