using System.Threading.Tasks;

namespace Badzeet.Domain.Book.Interfaces
{
    public interface IBookRepository
    {
        Task<Book.Model.Book> GetBook(long bookId);
        Task<Book.Model.Book> CreateBook(byte firstDayOfBudget);
    }
}
