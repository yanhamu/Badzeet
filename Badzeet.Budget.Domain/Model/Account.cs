using System;

namespace Badzeet.Budget.Domain.Model
{
    public class Account
    {
        public Guid Id { get; set; }
        public byte FirstDayOfTheBudget { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
