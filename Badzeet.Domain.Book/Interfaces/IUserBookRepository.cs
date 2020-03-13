using Badzeet.Domain.Book.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Domain.Book.Interfaces
{
    public interface IUserBookRepository
    {
        Task<IEnumerable<UserBook>> GetUsers(long bookId);
        Task<IEnumerable<UserBook>> GetBooks(Guid userId);
        Task<UserBook> Create(Guid userId, string username, long id);
    }
}
