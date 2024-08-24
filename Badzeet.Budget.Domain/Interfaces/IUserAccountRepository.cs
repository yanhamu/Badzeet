using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Badzeet.Budget.Domain.Model;

namespace Badzeet.Budget.Domain.Interfaces;

public interface IUserAccountRepository
{
    Task<IEnumerable<UserAccount>> GetUsers(long accountId);
    Task<IEnumerable<UserAccount>> GetUserAccounts(Guid userId);
    Task<UserAccount> Create(Guid userId, long accountId);
    Task Save();
}