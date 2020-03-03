using System.Collections.Generic;

namespace Badzeet.Web.Features.Book
{
    public class TransactionsModel
    {
        public IEnumerable<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
    }
}