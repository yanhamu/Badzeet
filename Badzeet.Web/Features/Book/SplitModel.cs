using System;

namespace Badzeet.Web.Features.Book
{
    public class SplitModel
    {
        public long OldTransactionId { get; set; }
        public decimal OldAmount { get; set; }
        public decimal NewAmount { get; set; }
        public long CategoryId { get; set; }
        public Guid OwnerId { get; set; }
        public string Description { get; set; }
    }
}