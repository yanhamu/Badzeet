using Badzeet.Budget.Domain.Model;
using System;
using System.Threading.Tasks;

namespace Badzeet.Budget.Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account?> GetAccount(Guid accountId);
        Task<Account> CreateAccount(byte firstDayOfBudget);
    }
}
