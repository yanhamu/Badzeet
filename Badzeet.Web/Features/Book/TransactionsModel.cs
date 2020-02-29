using System.Collections.Generic;

namespace Badzeet.Web.Features.Book
{
    public class TransactionsModel
    {
        public List<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
    }
}