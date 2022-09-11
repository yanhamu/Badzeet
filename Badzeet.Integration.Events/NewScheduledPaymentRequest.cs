using MediatR;
using System;

namespace Badzeet.Integration.Events
{
    public class NewScheduledPaymentRequest : IRequest
    {
        public NewScheduledPaymentRequest(long accountId, DateTime date, decimal amount, string description, Guid categoryId, Guid ownerId)
        {
            AccountId = accountId;
            Date = date;
            Amount = amount;
            Description = description;
            CategoryId = categoryId;
            OwnerId = ownerId;
        }

        public long AccountId { get; }
        public DateTime Date { get; }
        public decimal Amount { get; }
        public string Description { get; }
        public Guid CategoryId { get; }
        public Guid OwnerId { get; }
    }
}