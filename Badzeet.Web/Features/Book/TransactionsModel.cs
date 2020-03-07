using System.Collections.Generic;

namespace Badzeet.Web.Features.Book
{
    public class TransactionsModel
    {
        public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
        public IEnumerable<TransactionModel> Transactions { get; set; } = new List<TransactionModel>();
    }
}