using System;

namespace Badzeet.Domain.Budget.Model
{
    public class Account
    {
        public long Id { get; set; }
        public byte FirstDayOfTheBudget { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
