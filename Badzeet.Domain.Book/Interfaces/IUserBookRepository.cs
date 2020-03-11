using Badzeet.Domain.Book.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Domain.Book.Interfaces
{
    public interface IUserBookRepository
    {
        Task<IEnumerable<UserBook>> GetUsers(long bookId);
    }
}
