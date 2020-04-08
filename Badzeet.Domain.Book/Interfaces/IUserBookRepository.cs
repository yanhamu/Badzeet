using Badzeet.Domain.Book.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Domain.Book.Interfaces
{
    public interface IUserBookRepository
    {
        Task<IEnumerable<UserAccount>> GetUsers(long bookId);
        Task<IEnumerable<UserAccount>> GetBooks(Guid userId);
        Task<UserAccount> Create(Guid userId, long id);
    }
}
