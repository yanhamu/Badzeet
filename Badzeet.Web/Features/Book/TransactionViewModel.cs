using Badzeet.Domain.Book.Model;
using System.Collections.Generic;

namespace Badzeet.Web.Features.Book
{
    public class TransactionViewModel
    {
        public List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
        public TransactionModel Transaction { get; set; } = new TransactionModel();
        public IEnumerable<UserAccount> Users { get; set; } = new List<UserAccount>();
    }
}