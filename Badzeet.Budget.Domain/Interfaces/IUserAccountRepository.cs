using Badzeet.Budget.Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain.Interfaces
{
    public interface IUserAccountRepository
    {
        Task<IEnumerable<UserAccount>> GetUsers(Guid accountId);
        Task<IEnumerable<UserAccount>> GetUserAccounts(Guid userId);
        Task<UserAccount> Create(Guid userId, Guid id);
        Task Save();
    }
}
