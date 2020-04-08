using System.Threading.Tasks;

namespace Badzeet.Domain.Book.Interfaces
{
    public interface IBookRepository
    {
        Task<Book.Model.Account> GetBook(long bookId);
        Task<Book.Model.Account> CreateBook(byte firstDayOfBudget);
    }
}
