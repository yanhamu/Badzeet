using Badzeet.Budget.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain.Interfaces
{
    public interface IUserAccountRepository
    {
        Task<IEnumerable<UserAccount>> GetUsers(long accountId);
        Task<IEnumerable<UserAccount>> GetBooks(Guid userId);
        Task<UserAccount> Create(Guid userId, long id);
    }
}
